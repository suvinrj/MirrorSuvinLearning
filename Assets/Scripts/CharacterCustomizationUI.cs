using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;
using PsychoticLab; // ✅ Ensure correct namespace

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

        PopulateDropdowns();
        saveCharacterButton.onClick.AddListener(SaveCharacter);
    }

    // ✅ Populate dropdowns dynamically based on available character parts
    private void PopulateDropdowns()
{
    // ✅ Reset dropdowns before populating new options
    raceDropdown.ClearOptions();
    genderDropdown.ClearOptions();
    hairDropdown.ClearOptions();
    armorDropdown.ClearOptions();

    // ✅ Add new options dynamically
    raceDropdown.AddOptions(new List<string> { "Human", "Elf" });
    genderDropdown.AddOptions(new List<string> { "Male", "Female" });
    hairDropdown.AddOptions(GetHairOptions());
    armorDropdown.AddOptions(GetArmorOptions());

    // ✅ Ensure dropdown selects the first available valid option
    raceDropdown.value = 0;
    genderDropdown.value = 0;
    hairDropdown.value = (hairDropdown.options.Count > 1) ? 0 : -1;
    armorDropdown.value = (armorDropdown.options.Count > 1) ? 0 : -1;

    // ✅ Refresh the dropdowns
    raceDropdown.RefreshShownValue();
    genderDropdown.RefreshShownValue();
    hairDropdown.RefreshShownValue();
    armorDropdown.RefreshShownValue();

    // ✅ Attach event listeners
    raceDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });
    genderDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });
    hairDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });
    armorDropdown.onValueChanged.AddListener(delegate { UpdateCharacter(); });

    UpdateCharacter();
}




    

   private List<string> GetHairOptions()
{
    return new List<string>
    {
        "Bald",
        "Short Hair",
        "Medium Hair",
        "Long Hair",
        "Ponytail",
        "Braided Hair",
        "Spiky Hair",
        "Curly Hair",
        "Mohawk",
        "Sidecut",
        "Undercut",
        "Topknot",
        "Messy Hair",
        "Afro",
        "Bun",
        "Layered Hair",
        "Twintails",
        "Wavy Hair",
        "Shoulder-Length",
        "Dreadlocks",
        "Straight Long Hair",
        "Short Bangs",
        "Shaggy Hair",
        "Half-Up Half-Down",
        "Voluminous Curls",
        "Buzz Cut",
        "High Fade",
        "Braided Crown",
        "Side Part",
        "Messy Spikes",
        "Pixie Cut",
        "Chin-Length Bob",
        "Feathered Hair",
        "Anime Spikes",
        "Swept Back",
        "Faux Hawk",
        "Slicked Back",
        "Shaved Sides"
    };
}

    // ✅ Generate Armor Options
    private List<string> GetArmorOptions()
{
    return new List<string>
    {
        "Light Cloth",
        "Leather Armor",
        "Padded Armor",
        "Chainmail",
        "Scale Armor",
        "Knight Plate",
        "Heavy Plate Armor",
        "Royal Guard Armor",
        "Battle Mage Robes",
        "Assassin Outfit",
        "Ranger Gear",
        "Paladin Armor",
        "Barbarian Chestpiece",
        "Monk Robes",
        "Dark Sorcerer Cloak",
        "Samurai Armor",
        "Gladiator Gear",
        "Berserker Tunic",
        "Druid Robes",
        "Dragon Scale Armor",
        "Warlord Chestplate",
        "Elite Knight Armor",
        "Viking Battle Gear",
        "Templar Outfit",
        "Shadow Rogue Attire",
        "Necromancer Robes",
        "Shamanic Attire",
        "Nomad Outfit",
        "Arcane Enchanter Robes",
        "Royal Noble Attire",
        "Spartan Warrior Chestpiece"
    };
}



    // ✅ Update character in real time
    private void UpdateCharacter()
    {
        int race = raceDropdown.value;
        int gender = genderDropdown.value;
        int hair = hairDropdown.value;
        int armor = armorDropdown.value;

        Debug.Log($"🔄 Updating Character: Race={race}, Gender={gender}, Hair={hair}, Armor={armor}");
        
        characterRandomizer.SetCharacter(race, gender, hair, armor);
    }

    // ✅ Save character selections
    private void SaveCharacter()
    {
        PlayerPrefs.SetInt("CharacterRace", raceDropdown.value);
        PlayerPrefs.SetInt("CharacterGender", genderDropdown.value);
        PlayerPrefs.SetInt("CharacterHair", hairDropdown.value);
        PlayerPrefs.SetInt("CharacterArmor", armorDropdown.value);
        PlayerPrefs.Save();

        Debug.Log("✅ Character Saved!");

        // ✅ Move to Next Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerSetupScene"); // Scene 2
    }
}
