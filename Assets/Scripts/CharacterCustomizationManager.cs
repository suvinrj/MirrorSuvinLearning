using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class CharacterCustomizationManager : NetworkBehaviour
{
    public static CharacterCustomizationManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void FinishCustomization()
    {
        if (isServer)
        {
            Debug.Log("✅ All players finished customization! Moving to Game Scene...");
            SceneManager.LoadScene("MirrorRoomOnline"); // ✅ Move to the game scene
        }
    }
}
