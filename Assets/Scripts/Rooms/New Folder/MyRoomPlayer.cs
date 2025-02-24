using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyRoomPlayer : NetworkRoomPlayer
{
    // Example of storing extra data in the room player
    [SyncVar]
    public string displayName;

   public override void OnStartClient()
{
    base.OnStartClient();
    
    if (SceneManager.GetActiveScene().name != CustomRoomManager.singleton.RoomScene)
    {
        Debug.LogError("❌ Client was spawned in the wrong scene! Stopping spawn.");
        return;
    }

    Debug.Log("✅ RoomPlayer spawned in the correct LOBBY scene.");
}


public override void OnClientEnterRoom()
{
    base.OnClientEnterRoom();
    Debug.Log($"✅ Player {displayName} joined the lobby.");

    // Ensure lobby updates when client joins
    if (LobbyUI.instance != null)
    {
        LobbyUI.instance.RefreshPlayerList();
    }
}

public override void OnClientExitRoom()
{
    base.OnClientExitRoom();
    Debug.Log($"{displayName} left the room.");
    LobbyUI.instance.RefreshPlayerList();
}

public void ToggleReady()
{
    Debug.Log($"🟡 {displayName} toggling ready state from {readyToBegin} to {!readyToBegin}");
    CmdChangeReadyState(!readyToBegin);
}

}
