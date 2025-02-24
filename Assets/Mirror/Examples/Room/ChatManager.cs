using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Examples.NetworkRoom;
using System.Collections;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;

    public Text chatDisplay;
    public InputField chatInput;
    
    private NetworkRoomPlayerExt localPlayer; // ✅ Store reference to avoid searching every time

    private void Awake()
    {
        instance = this;
    }

    public override void OnStartClient()
    {
        StartCoroutine(FindLocalPlayer());
    }

    private IEnumerator FindLocalPlayer()
    {
        while (localPlayer == null)
        {
            if (NetworkClient.ready && NetworkClient.localPlayer != null)
            {
                localPlayer = NetworkClient.localPlayer.GetComponent<NetworkRoomPlayerExt>();
            }

            if (localPlayer == null)
            {
                Debug.LogWarning("⚠️ Waiting for local player...");
                yield return new WaitForSeconds(0.5f);
            }
        }

        Debug.Log($"✅ Local player found: {localPlayer.playerName}");
    }

    public void SendChatMessage()
    {
        if (string.IsNullOrWhiteSpace(chatInput.text)) return;

        if (localPlayer == null)
        {
            Debug.LogError("⚠️ Local player not found in room! Chat not sent.");
            return;
        }

        string message = chatInput.text;

        // ✅ Send the chat message using `CmdSendMessage()`
        localPlayer.CmdSendMessage(message);

        ClearInput();
    }

    public void AppendMessage(string message)
    {
        chatDisplay.text += message + "\n";
    }

    public void ClearInput()
    {
        if (chatInput != null)
        {
            chatInput.text = "";
        }
    }
}
