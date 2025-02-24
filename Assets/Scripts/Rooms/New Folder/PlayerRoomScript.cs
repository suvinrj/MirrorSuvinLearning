using UnityEngine;
using Mirror;

public class PlayerRoomScript : NetworkBehaviour
{
    [Command]
    public void CmdCreateRoom(string roomName)
    {
        // This runs on the server
        NetworkConnectionToClient conn = connectionToClient; // This player's connection

        CustomNetworkManager mgr = (CustomNetworkManager)NetworkManager.singleton;
        mgr.CreateRoomRequest(roomName, conn);
    }
}
