using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public Text playerInfoText; // ✅ UI Text to display player name + Ready Status

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log($"Player {base.playerName} has joined the room."); // ✅ Use base.playerName

            if (!isLocalPlayer) return;

            // ✅ Assign the saved name when joining the room
            if (PlayerPrefs.HasKey("PlayerName"))
            {
                string savedName = PlayerPrefs.GetString("PlayerName");
                CmdSetPlayerName(savedName);
                Debug.Log($"✅ Setting Player Name from PlayerPrefs: {savedName}");
            }

            UpdatePlayerUI();
        }

        public override void ReadyStateChanged(bool _, bool newReadyState)
        {
            base.ReadyStateChanged(_, newReadyState);
            UpdatePlayerUI();
        }

        private void UpdatePlayerUI()
        {
            if (playerInfoText != null)
            {
                playerInfoText.text = $"{base.playerName} {(readyToBegin ? "(Ready)" : "(Not Ready)")}"; // ✅ Use base.playerName
            }
            else
            {
                Debug.LogError("⚠️ playerInfoText is not assigned in the Inspector!");
            }
        }

        // ✅ Fix: Call base.SetPlayerName() instead of modifying SyncVar
        [Command]
        public void CmdSetPlayerName(string newName)
        {
            Debug.Log($"🔄 Syncing player name: {newName}");
            base.SetPlayerName(newName); // ✅ Use base function
        }

        // ✅ Fix: Add CmdSendMessage function for chat support
        [Command(requiresAuthority = false)]
        
public void CmdSendMessage(string message)
{
    RpcReceiveMessage($"{playerName}: {message}"); // ✅ Only send once to clients
}

[ClientRpc]
private void RpcReceiveMessage(string message)
{
    // ✅ Prevent host from seeing the message twice
    if (isServer && isLocalPlayer) return;

    ChatManager.instance.AppendMessage(message);
}
    }
}
