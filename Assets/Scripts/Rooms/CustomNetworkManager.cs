using UnityEngine;
using Mirror;
using System.Collections.Generic;

// A simple Room class: roomName, host, list of players, open/closed

public class CustomNetworkManager : NetworkManager
{
    // Keep track of all active rooms on the server
    public Dictionary<string, Room> activeRooms = new Dictionary<string, Room>();

    // Called when a client connects to the server
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Client connected: {conn.connectionId}");
    }

    // Called when a client disconnects from the server
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log($"Client disconnected: {conn.connectionId}");

        // Optionally: remove them from their room, if they're in one
        // ...
    }

    // Create a new room. Must be called on the server side (or via a Command).
    [Server]
    public void CreateRoomRequest(string roomName, NetworkConnectionToClient conn)
    {
        if (activeRooms.ContainsKey(roomName))
        {
            Debug.Log($"Cannot create room: '{roomName}' already exists!");
            return;
        }

        Room newRoom = new Room(roomName, conn);
        activeRooms.Add(roomName, newRoom);

        Debug.Log($"Room '{roomName}' created by connectionId: {conn.connectionId}");

        // Notify all clients of the updated room list
        UpdateAllClientsRoomList();
    }

    // Add the client to an existing room
    [Server]
    public void JoinRoomRequest(string roomName, NetworkConnectionToClient conn)
    {
        if (activeRooms.TryGetValue(roomName, out Room room))
        {
            if (room.isOpen)
            {
                room.players.Add(conn);
                Debug.Log($"Client {conn.connectionId} joined Room '{roomName}'");
            }
            else
            {
                Debug.Log($"Room '{roomName}' is closed.");
            }
        }
        else
        {
            Debug.Log($"Join failed: Room '{roomName}' not found!");
        }

        // Notify all clients of the updated room list
        UpdateAllClientsRoomList();
    }

    // Example method to broadcast or sync the active rooms
    [Server]
    public void UpdateAllClientsRoomList()
    {
        // Here you'd convert 'activeRooms' into a serializable data structure
        // and send it to all clients via a custom message or using Mirror Observers.
        // For now, we just log it:
        Debug.Log("Updating all clients with new room list...");
    }

    // ✅ Move *all* connected clients to the "LobbyScene"
    [Server]
    public void GoToLobbyForAllPlayers()
    {
        // Mirror automatically calls OnServerSceneChanged on the server
        // and moves all clients to the specified scene
        Debug.Log("Moving all players to LobbyScene...");
        ServerChangeScene("LobbyScene");
    }
}
