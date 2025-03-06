using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;
using PsychoticLab;

public class CharacterCustomizationUI : MonoBehaviour
{
    public static CharacterCustomizationUI instance;

    [Header("UI Elements")]
    public Dropdown raceDropdown;
    public Dropdown genderDropdown;
    public Dropdown hairDropdown;
    public Dropdown armorDropdown;
    public Button saveCharacterButton;

    private CharacterRandomizer characterRandomizer;

    private void Awake()
    {
        instance = this;
        characterRandomizer = FindObjectOfType<CharacterRandomizer>();

        if (characterRandomizer == null)
        {
            Debug.LogError("❌ CharacterRandomizer not found! Customization will not work.");
            return;
        }

        PopulateDropdowns();
        saveCharacterButton.onClick.AddListener(SaveCharacter);
    }

    // ✅ Populate dropdowns dynamically based on available models
    private void PopulateDropdowns()
    {
        raceDropdown.ClearOptions();
        genderDropdown.ClearOptions();
        hairDropdown.ClearOptions();
        armorDropdown.ClearOptions();

        raceDropdown.AddOptions(new List<string> { "Human", "Elf" });
        genderDropdown.AddOptions(new List<string> { "Male", "Female" });
        hairDropdown.AddOptions(GetHairOptions());
        armorDropdown.AddOptions(GetArmorOptions());

        raceDropdown.value = 0;
        genderDropdown.value = 0;
        hairDropdown.value = 0;
        armorDropdown.value = 0;

        raceDropdown.RefreshShownValue();
        genderDropdown.RefreshShownValue();
        hairDropdown.RefreshShownValue();
        armorDropdown.RefreshShownValue();

        raceDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });
        genderDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });
        hairDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });
        armorDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });

        UpdateCharacter();
    }

    // ✅ Generate Hair Options (Limited to Five)
    private List<string> GetHairOptions()
    {
        return new List<string>
        {
            "Short Hair",
            "Long Hair",
            "Ponytail",
            "Bald",
            "Braided Hair"
        };
    }

    // ✅ Armor options now change **body parts** dynamically
    private List<string> GetArmorOptions()
    {
        return new List<string>
        {
            "Light Cloth",
            "Leather Armor",
            "Padded Armor",
            "Chainmail",
            "Plate Armor"
        };
    }

    // ✅ Apply the selected customization to the character
    private void UpdateCharacter()
    {
        if (characterRandomizer == null)
        {
            Debug.LogError("❌ CharacterRandomizer not found! Cannot update character.");
            return;
        }

        int race = raceDropdown.value;
        int gender = genderDropdown.value;
        int hair = hairDropdown.value;
        int armor = armorDropdown.value;

        Debug.Log($"🔄 Updating Character: Race={race}, Gender={gender}, Hair={hair}, Armor={armor}");

        characterRandomizer.SetCharacter(race, gender, hair, armor);
        ApplyArmor(armor, gender, race);
    }

    // ✅ Apply armor by enabling/disabling body parts
    private void ApplyArmor(int armor, int gender, int race)
    {
        CharacterObjectGroups characterParts = (gender == 0) ? characterRandomizer.male : characterRandomizer.female;

        // Disable all previous body parts (EXCEPT Head & Hands)
        characterRandomizer.enabledObjects.ForEach(part => part.SetActive(false));
        characterRandomizer.enabledObjects.Clear();

        // ✅ Keep Head Always Active
        ActivateItem(characterParts.headAllElements[0]);

        // ✅ If Race is Elf, Enable Elf Ears
        if (race == 1 && characterRandomizer.allGender.elf_Ear.Count > 0)
            ActivateItem(characterRandomizer.allGender.elf_Ear[0]);

        // ✅ Keep Hands Active
        ActivateItem(characterParts.hand_Right[0]);
        ActivateItem(characterParts.hand_Left[0]);

        // ✅ Apply Armor Set (Torso, Arms, Hips, Legs)
        switch (armor)
        {
            case 0: // Light Cloth
                ActivateItem(characterParts.torso[0]);
                ActivateItem(characterParts.arm_Upper_Right[0]);
                ActivateItem(characterParts.arm_Upper_Left[0]);
                ActivateItem(characterParts.arm_Lower_Right[0]);
                ActivateItem(characterParts.arm_Lower_Left[0]);
                ActivateItem(characterParts.hips[0]);
                ActivateItem(characterParts.leg_Right[0]);
                ActivateItem(characterParts.leg_Left[0]);
                break;

            case 1: // Leather Armor
                ActivateItem(characterParts.torso[1]);
                ActivateItem(characterParts.arm_Upper_Right[1]);
                ActivateItem(characterParts.arm_Upper_Left[1]);
                ActivateItem(characterParts.arm_Lower_Right[1]);
                ActivateItem(characterParts.arm_Lower_Left[1]);
                ActivateItem(characterParts.hips[1]);
                ActivateItem(characterParts.leg_Right[1]);
                ActivateItem(characterParts.leg_Left[1]);
                break;

            case 2: // Padded Armor
                ActivateItem(characterParts.torso[2]);
                ActivateItem(characterParts.arm_Upper_Right[2]);
                ActivateItem(characterParts.arm_Upper_Left[2]);
                ActivateItem(characterParts.arm_Lower_Right[2]);
                ActivateItem(characterParts.arm_Lower_Left[2]);
                ActivateItem(characterParts.hips[2]);
                ActivateItem(characterParts.leg_Right[2]);
                ActivateItem(characterParts.leg_Left[2]);
                break;

            case 3: // Chainmail
                ActivateItem(characterParts.torso[3]);
                ActivateItem(characterParts.arm_Upper_Right[3]);
                ActivateItem(characterParts.arm_Upper_Left[3]);
                ActivateItem(characterParts.arm_Lower_Right[3]);
                ActivateItem(characterParts.arm_Lower_Left[3]);
                ActivateItem(characterParts.hips[3]);
                ActivateItem(characterParts.leg_Right[3]);
                ActivateItem(characterParts.leg_Left[3]);
                break;

            case 4: // Plate Armor
                ActivateItem(characterParts.torso[4]);
                ActivateItem(characterParts.arm_Upper_Right[4]);
                ActivateItem(characterParts.arm_Upper_Left[4]);
                ActivateItem(characterParts.arm_Lower_Right[4]);
                ActivateItem(characterParts.arm_Lower_Left[4]);
                ActivateItem(characterParts.hips[4]);
                ActivateItem(characterParts.leg_Right[4]);
                ActivateItem(characterParts.leg_Left[4]);
                break;
        }

        Debug.Log($"✔️ Armor Applied: {armor}");
    }

    void ApplyCustomization()
{
    if (PlayerCustomization.localPlayer == null)
    {
        Debug.LogError("❌ Local Player not found! Customization cannot be applied.");
        return;
    }

    int selectedRace = raceDropdown.value;
    int selectedGender = genderDropdown.value;
    int selectedHair = hairDropdown.value;
    int selectedArmor = armorDropdown.value;

    // ✅ Apply customization across the network
    PlayerCustomization.localPlayer.CmdSetCustomization(selectedRace, selectedGender, selectedHair, selectedArmor);
    PlayerCustomization.localPlayer.CmdSetArmor(selectedArmor); // ✅ Apply Armor
}



    // ✅ Enable and track selected parts
    private void ActivateItem(GameObject go)
    {
        go.SetActive(true);
        characterRandomizer.enabledObjects.Add(go);
    }

    // ✅ Save character selections and move to the next scene
    private void SaveCharacter()
    {
        PlayerPrefs.SetInt("CharacterRace", raceDropdown.value);
        PlayerPrefs.SetInt("CharacterGender", genderDropdown.value);
        PlayerPrefs.SetInt("CharacterHair", hairDropdown.value);
        PlayerPrefs.SetInt("CharacterArmor", armorDropdown.value);
        PlayerPrefs.Save();

        Debug.Log("✅ Character Saved!");

        // ✅ Move to Game Scene after customization
        UnityEngine.SceneManagement.SceneManager.LoadScene("MirrorRoomOffline");
    }
}
