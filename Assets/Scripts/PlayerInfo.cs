using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour
{
    public static PlayerInfo localPlayer;

    [SyncVar(hook = nameof(OnNameChanged))] 
    public string playerName = "Player";

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    public Text playerNameText;
    public Renderer playerRenderer;

    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
    }

    public override void OnStartLocalPlayer()
    {
        playerName = UIManager.instance.nameInputField.text;

        // ✅ Fix: Remove reference to `colorDropdown`
        Color selectedColor = Color.white; // Default to white or another color
        
        CmdSetPlayerInfo(playerName, selectedColor);
    }

    [Command]
    public void CmdSetPlayerInfo(string newName, Color newColor)
    {
        playerName = newName;
        playerColor = newColor;
    }

    private void OnNameChanged(string oldName, string newName)
    {
        if (playerNameText != null)
        {
            playerNameText.text = newName;
        }
    }

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = newColor;
        }
    }
}
