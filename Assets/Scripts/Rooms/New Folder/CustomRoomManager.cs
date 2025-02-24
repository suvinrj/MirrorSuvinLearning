using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class CustomRoomManager : NetworkRoomManager
{


// Make a new static singleton reference typed as CustomRoomManager
    public static new CustomRoomManager singleton;

    public override void Awake()
    {
        // Important to call base first
        base.Awake();
        singleton = this; // Assign the singleton to this instance
    }



  IEnumerator WaitAndUpdateLobby()
{
    yield return new WaitForSeconds(0.5f); // Small delay
    if (LobbyUI.instance != null)
    {
        Debug.Log("🔄 Refreshing lobby UI after delay...");
        LobbyUI.instance.RefreshPlayerList();
    }
    else
    {
        Debug.LogError("❌ Still no LobbyUI instance after delay!");
    }
}



public override void OnRoomServerConnect(NetworkConnectionToClient conn)
{
    base.OnRoomServerConnect(conn);

    Debug.Log($"🔗 Client {conn.connectionId} connected to the host! Total Players: {roomSlots.Count}");
}


    // Called on the server when a player leaves the room
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);
        Debug.Log("A player disconnected from the lobby.");
    }

       // Called when all players in the room are ready
    public override void OnRoomServerPlayersReady()
{
    // base.OnRoomServerPlayersReady();  // Comment this out
    Debug.Log("All players are ready, waiting for host to start game manually...");
}



public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
{
    Debug.Log($"👤 Adding new player for Connection ID: {conn.connectionId}");

    if (roomPlayerPrefab == null)
    {
        Debug.LogError("❌ Room Player Prefab is NULL! Cannot add player.");
        return;
    }

NetworkRoomPlayer newPlayer = Instantiate(roomPlayerPrefab);
NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject);

    Debug.Log($"✅ Player {conn.connectionId} added to the room.");
}


public override void OnStartHost()
{
    base.OnStartHost();
    Debug.Log("✅ OnStartHost() triggered! Host is now active.");
}

    // Called on the client when they enter the room
    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
        Debug.Log("Client entered the lobby scene.");
    }

    // Called on the client when they exit the room
    public override void OnRoomClientExit()
    {
        base.OnRoomClientExit();
        Debug.Log("Client left the lobby scene.");
    }

public override void OnStartServer()
{
    base.OnStartServer();

    if (!NetworkClient.active)
    {
        Debug.Log("🟢 No active client detected. Starting Host from CustomRoomManager...");
        StartHost();
    }
}

public override void ServerChangeScene(string newSceneName)
{
    Debug.Log($"🔄 Changing scene to: {newSceneName}");

    if (!NetworkServer.active)
    {
        Debug.LogError("❌ ServerChangeScene called, but NO ACTIVE SERVER!");
        return;
    }

    // Make sure clients move to the new scene
    foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
    {
        Debug.Log($"🎯 Moving Client {conn.connectionId} to {newSceneName}");
    }

    base.ServerChangeScene(newSceneName);
}


   public override void OnRoomServerSceneChanged(string sceneName)
{
    base.OnRoomServerSceneChanged(sceneName);

    if (sceneName == GameplayScene)
    {
        Debug.Log("🚀 Spawning players in the gameplay scene...");

        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            GameObject playerInstance = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, playerInstance);
        }
    }
    else
    {
        Debug.Log("🏠 We are still in the Lobby. Players should NOT spawn yet.");
    }
}

}
