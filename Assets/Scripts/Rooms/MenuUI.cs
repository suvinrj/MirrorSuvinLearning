using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections;

public class MenuUI : MonoBehaviour
{
    public void OnCreateRoomButton()
    {
        Debug.Log("🟡 Creating room and switching to Lobby scene...");
        SceneManager.LoadScene("Lobby");
        StartCoroutine(WaitAndStartHost());
    }

    IEnumerator WaitAndStartHost()
    {
        yield return new WaitForSeconds(1f); // Ensure the scene loads before continuing

        Debug.Log("🟢 Attempting to Start Host...");

        if (CustomRoomManager.singleton == null)
        {
            Debug.LogError("❌ CustomRoomManager.singleton is NULL! Host cannot start.");
            yield break;
        }

        CustomRoomManager.singleton.StartHost();
        Debug.Log("✅ StartHost() called successfully.");
    }


    public void OnJoinRoomButton()
{
    Debug.Log("🟡 Join Room button clicked. Loading Lobby scene...");
    SceneManager.LoadScene("Lobby");
    StartCoroutine(WaitAndJoinRoom());
}

IEnumerator WaitAndJoinRoom()
{
    yield return new WaitForSeconds(1f); // Ensure scene loads first

    Debug.Log("🟢 Attempting to join as Client...");

    if (CustomRoomManager.singleton == null)
    {
        Debug.LogError("❌ CustomRoomManager.singleton is NULL! Cannot join room.");
        yield break;
    }

    // Set the server IP (change this if needed)
    CustomRoomManager.singleton.networkAddress = "localhost"; 

    CustomRoomManager.singleton.StartClient();
    Debug.Log("✅ StartClient() called successfully.");
}
}
