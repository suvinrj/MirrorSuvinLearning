using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;
using System.Text;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI instance;

    [Header("UI References")]
    public Text playerListText; // Legacy UI Text
    public Button readyButton;
    public Button startGameButton;

   private void Awake()
{
    // Instead of only showing once, do:
    // Show the button if we're the host after the server starts
    UpdateStartButton();
}

public void UpdateStartButton()
{
    // Show start button if we are the host
    // (NetworkServer.active is true for the host)
    bool isHost = NetworkServer.active && NetworkClient.active;
    startGameButton.gameObject.SetActive(isHost);
}

 private void Start()
{
    Debug.Log("🏁 LobbyUI script started.");

    // ✅ Only start the host if it's not running already
    if (!NetworkClient.active && !NetworkServer.active)
    {
        Debug.Log("🟢 No active client detected. Attempting to start Host...");
        CustomRoomManager.singleton.StartHost();
    }
    else
    {
        Debug.Log("⚠️ Host or Client already running. Not starting again.");
    }

    RefreshPlayerList();
}


    private void OnReadyButtonClicked()
    {
        var localRoomPlayer = FindLocalRoomPlayer();
        if (localRoomPlayer != null) localRoomPlayer.ToggleReady();
    }

public void OnStartGameClicked()
{
    Debug.Log("🟢 Start Game button clicked!");

    if (NetworkServer.active)
    {
        Debug.Log("🚀 Host is starting the game...");
        CustomRoomManager.singleton.ServerChangeScene(CustomRoomManager.singleton.GameplayScene);
    }
    else
    {
        Debug.LogError("❌ Start Game clicked, but NOT the host!");
    }
}


    private MyRoomPlayer FindLocalRoomPlayer()
    {
        // Use CustomRoomManager.singleton
        foreach (var roomPlayer in CustomRoomManager.singleton.roomSlots)
        {
            if (roomPlayer.isLocalPlayer)
                return (MyRoomPlayer)roomPlayer;
        }
        return null;
    }

public void RefreshPlayerList()
{
    StringBuilder sb = new StringBuilder();
    bool allReady = true;

    foreach (var roomPlayer in CustomRoomManager.singleton.roomSlots)
    {
        var customPlayer = roomPlayer as MyRoomPlayer;
        if (customPlayer != null)
        {
            Debug.Log($"🎭 Player: {customPlayer.displayName}, Ready: {customPlayer.readyToBegin}");
            sb.AppendLine($"{customPlayer.displayName} (Ready: {customPlayer.readyToBegin})");

            if (!customPlayer.readyToBegin) allReady = false;
        }
    }

    playerListText.text = sb.ToString();
    bool isHost = NetworkServer.active;
    startGameButton.gameObject.SetActive(isHost);
    startGameButton.interactable = isHost && allReady;
}
}
