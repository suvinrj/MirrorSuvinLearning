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

    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }

        characterRandomizer = GetComponent<CharacterRandomizer>();
        ApplyCustomization();
    }

    [Command]
    public void CmdSetCustomization(int race, int gender, int hair, int armor)
    {
        selectedRace = race;
        selectedGender = gender;
        selectedHair = hair;
        selectedArmor = armor;

        RpcApplyCustomization(selectedRace, selectedGender, selectedHair, selectedArmor);
    }

    [ClientRpc]
    void RpcApplyCustomization(int race, int gender, int hair, int armor)
    {
        characterRandomizer.SetCharacter(race, gender, hair, armor);
    }

    void ApplyCustomization()
    {
        characterRandomizer.SetCharacter(selectedRace, selectedGender, selectedHair, selectedArmor);
    }
}
