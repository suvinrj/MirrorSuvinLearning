using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CharacterCustomizationUI : MonoBehaviour
{
    public static CharacterCustomizationUI instance;

    [Header("UI Elements")]
    public Dropdown raceDropdown;
    public Dropdown genderDropdown;
    public Dropdown hairDropdown;
    public Dropdown armorDropdown;
    public Button applyButton;

    private void Awake()
    {
        instance = this;
        applyButton.onClick.AddListener(ApplyCustomization);
    }

    void ApplyCustomization()
    {
        int selectedRace = raceDropdown.value;
        int selectedGender = genderDropdown.value;
        int selectedHair = hairDropdown.value;
        int selectedArmor = armorDropdown.value;

        // Apply character customization
        PlayerCustomization.localPlayer.CmdSetCustomization(selectedRace, selectedGender, selectedHair, selectedArmor);
    }
}
