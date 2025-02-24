using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar]
        public int index;

        [SyncVar]
        public string playerName; // ✅ Sync player name into game scene

        [SyncVar]
        public uint score;

        void OnGUI()
        {
            GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"{playerName}: {score:0000000}");
        }
    }
}
