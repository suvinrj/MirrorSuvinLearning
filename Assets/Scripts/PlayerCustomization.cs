using UnityEngine;
using Mirror;
using PsychoticLab;

public class PlayerCustomization : NetworkBehaviour
{
    public static PlayerCustomization localPlayer;

    [Header("Customization Options")]
    [SyncVar] public int selectedRace;
    [SyncVar] public int selectedGender;
    [SyncVar] public int selectedHair;
    [SyncVar] public int selectedArmor;

    private CharacterRandomizer characterRandomizer;

     public override void OnStartClient()
    {
        base.OnStartClient();
        characterRandomizer = GetComponent<CharacterRandomizer>();

        if (isLocalPlayer)
        {
            ApplyCharacterCustomization();
        }
    }

    void ApplyCharacterCustomization()
    {
        int race = PlayerPrefs.GetInt("SelectedRace", 0);
        int gender = PlayerPrefs.GetInt("SelectedGender", 0);
        int hair = PlayerPrefs.GetInt("SelectedHair", 0);
        int armor = PlayerPrefs.GetInt("SelectedArmor", 0);

        Debug.Log($"🎮 Applying Game Character: Race={race}, Gender={gender}, Hair={hair}, Armor={armor}");

        // Apply character model
        characterRandomizer.SetCharacter(race, gender, hair, armor);
    }


    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }

        characterRandomizer = GetComponent<CharacterRandomizer>();
        ApplyCustomization(); // ✅ Apply customization when the game starts
    }

    [Command]

public void CmdSetCustomization(int race, int gender, int hair, int armor)
{
    selectedRace = race;
    selectedGender = gender;
    selectedHair = hair;
    selectedArmor = armor;

    Debug.Log($"🔄 Syncing Customization: Race={race}, Gender={gender}, Hair={hair}, Armor={armor}");

    RpcApplyCustomization(selectedRace, selectedGender, selectedHair, selectedArmor);
}

    [ClientRpc]
    void RpcApplyCustomization(int race, int gender, int hair, int armor)
    {
        if (characterRandomizer != null)
        {
            characterRandomizer.SetCharacter(race, gender, hair, armor);
        }
    }

    void ApplyCustomization()
    {
        if (characterRandomizer != null)
        {
            characterRandomizer.SetCharacter(selectedRace, selectedGender, selectedHair, selectedArmor);
        }
    }
}
