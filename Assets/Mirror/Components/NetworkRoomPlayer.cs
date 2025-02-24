using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Room Player")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-room-player")]
    public class NetworkRoomPlayer : NetworkBehaviour
    {
        [Tooltip("This flag controls whether the default UI is shown for the room player")]
        public bool showRoomGUI = true;

        [Header("Diagnostics")]
        [ReadOnly, Tooltip("Diagnostic flag indicating whether this player is ready for the game to begin")]
        [SyncVar(hook = nameof(ReadyStateChanged))]
        public bool readyToBegin;

        [ReadOnly, Tooltip("Diagnostic index of the player, e.g. Player1, Player2, etc.")]
        [SyncVar(hook = nameof(IndexChanged))]
        public int index;

        // ✅ Ensure the name can be changed safely from another script
        [SyncVar]
        public string playerName = "Player";

        public void SetPlayerName(string newName)
        {
            playerName = newName;
        }

        #region Unity Callbacks

        public virtual void Start()
        {
            if (NetworkManager.singleton is NetworkRoomManager room)
            {
                if (room.dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);

                room.roomSlots.Add(this);

                if (NetworkServer.active)
                    room.RecalculateRoomPlayerIndices();

                if (NetworkClient.active)
                    room.CallOnClientEnterRoom();

                // ✅ Set the player name when they join
                if (isLocalPlayer && PlayerPrefs.HasKey("PlayerName"))
                {
                    CmdSetPlayerName(PlayerPrefs.GetString("PlayerName"));
                }
            }
            else Debug.LogError("RoomPlayer could not find a NetworkRoomManager.");
        }

        public virtual void OnDisable()
        {
            if (NetworkClient.active && NetworkManager.singleton is NetworkRoomManager room)
            {
                room.roomSlots.Remove(this);
                room.CallOnClientExitRoom();
            }
        }

        #endregion

        #region Commands

        [Command]
        public void CmdChangeReadyState(bool readyState)
        {
            readyToBegin = readyState;
            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room != null)
            {
                room.ReadyStatusChanged();
            }
        }

        [Command]
        public void CmdSetPlayerName(string newName)
        {
            playerName = newName;
        }

        #endregion

        #region SyncVar Hooks

        public virtual void IndexChanged(int oldIndex, int newIndex) {}

        public virtual void ReadyStateChanged(bool oldReadyState, bool newReadyState) {}

        private void OnNameChanged(string oldName, string newName)
        {
            Debug.Log($"🔄 Player name changed: {oldName} → {newName}");
        }

        #endregion

        #region Room Client Virtuals

        public virtual void OnClientEnterRoom() {}

        public virtual void OnClientExitRoom() {}

        #endregion

        #region Optional UI

        public virtual void OnGUI()
        {
            if (!showRoomGUI) return;

            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                if (!room.showRoomGUI) return;
                if (!Utils.IsSceneActive(room.RoomScene)) return;

                DrawPlayerReadyState();
                DrawPlayerReadyButton();
            }
        }

        void DrawPlayerReadyState()
        {
            GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

            // ✅ Show the actual player name instead of "Player [1]"
            GUILayout.Label($"{playerName}");

            if (readyToBegin)
                GUILayout.Label("Ready");
            else
                GUILayout.Label("Not Ready");

            if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
            {
                GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
            }

            GUILayout.EndArea();
        }

        void DrawPlayerReadyButton()
        {
            if (NetworkClient.active && isLocalPlayer)
            {
                GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));

                if (readyToBegin)
                {
                    if (GUILayout.Button("Cancel"))
                        CmdChangeReadyState(false);
                }
                else
                {
                    if (GUILayout.Button("Ready"))
                        CmdChangeReadyState(true);
                }

                GUILayout.EndArea();
            }
        }

        #endregion
    }
}
