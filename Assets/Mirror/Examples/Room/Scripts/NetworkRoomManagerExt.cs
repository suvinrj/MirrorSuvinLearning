using UnityEngine;
using Mirror;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomManagerExt : NetworkRoomManager
    {
        [Header("Spawner Setup")]
        public GameObject rewardPrefab; // ✅ Fix: Add reward prefab

        public static new NetworkRoomManagerExt singleton => NetworkManager.singleton as NetworkRoomManagerExt;

        public override void OnRoomServerPlayersReady()
        {
            if (NetworkServer.active && allPlayersReady)
            {
                ServerChangeScene(GameplayScene);
            }
        }
    }
}
