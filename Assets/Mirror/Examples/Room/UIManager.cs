using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement; // ✅ Required for scene switching
using Mirror.Examples.NetworkRoom;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Player Setup")]
    public InputField nameInputField; // ✅ Legacy InputField
    public Dropdown teamDropdown; // ✅ If you are using a dropdown for team selection
    public Button submitNameButton; // ✅ Button to store name and go to Scene 2

    [Header("Room Setup (Scene 2)")]
    public InputField roomNameInputField; // ✅ Legacy InputField for room name
    public Button createRoomButton;
    public Button joinRoomButton;

    private string playerName = "";

    private void Awake()
    {
        instance = this;
        
        // Assign button actions
        submitNameButton.onClick.AddListener(SubmitPlayerName);
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    public void SubmitPlayerName()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            Debug.LogError("Player name cannot be empty!");
            return;
        }

        playerName = nameInputField.text; // ✅ Store name
        PlayerPrefs.SetString("PlayerName", playerName); // ✅ Save name for later use
                    Debug.Log("Player name submitted!");

    }

    public void CreateRoom()
    {
        if (string.IsNullOrWhiteSpace(roomNameInputField.text))
        {
            Debug.LogError("Room name cannot be empty!");
            return;
        }

        // ✅ Start as Host
        NetworkRoomManagerExt.singleton.StartHost();
                SceneManager.LoadScene("MirrorRoomOnline"); // ✅ Move to Scene 2 (Lobby)
                    Debug.Log("Room name submitted!");

    }

    public void JoinRoom()
    {
        

        // ✅ Set the server IP or match system (localhost for testing)
        NetworkRoomManagerExt.singleton.networkAddress = "localhost"; 
        NetworkRoomManagerExt.singleton.StartClient();
                            Debug.Log("Joining room!");

                SceneManager.LoadScene("MirrorRoomOnline"); // ✅ Move to Scene 2 (Lobby)

    }
}
