using UnityEngine;
using Mirror;

public class PlayerChat : NetworkBehaviour
{
    public static PlayerChat localPlayer;
    private PlayerInfo playerInfo; // ✅ Reference to get the player name

    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }

        // ✅ Get the PlayerInfo component to access the player name
        playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string message = ChatManager.instance != null ? ChatManager.instance.GetComponentInChildren<UnityEngine.UI.InputField>().text : "";
            if (!string.IsNullOrWhiteSpace(message))
            {
                TrySendMessage(message);
                ChatManager.instance?.ClearInput();
            }
        }
    }

    /// <summary>
    /// Called by ChatManager when the user clicks 'Send'
    /// </summary>
    public void TrySendMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            // ✅ Use player name instead of ID
            string playerName = playerInfo != null ? playerInfo.playerName : $"Player {netId}";
            CmdSendMessage($"{playerName}: {message}");
        }
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcReceiveMessage(message);
    }

    [ClientRpc]
    private void RpcReceiveMessage(string message)
    {
        ChatManager.instance.AppendMessage(message);
    }
}
