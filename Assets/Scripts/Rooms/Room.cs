using System.Collections.Generic;
using Mirror;

public class Room
{
    public string roomName;
    public NetworkConnectionToClient host; 
    public List<NetworkConnectionToClient> players = new List<NetworkConnectionToClient>();
    public bool isOpen = true; // can set false if the game starts

    public Room(string name, NetworkConnectionToClient hostConn)
    {
        roomName = name;
        host = hostConn;
        players.Add(hostConn);
    }
}
