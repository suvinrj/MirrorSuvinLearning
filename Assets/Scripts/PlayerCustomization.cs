using UnityEngine;
using Mirror;
using PsychoticLab;

public class PlayerCustomization : NetworkBehaviour
{
    public static PlayerCustomization localPlayer;



    [Header("Customization Options")]
    [SyncVar(hook = nameof(UpdateRace))] public int selectedRace;
    [SyncVar(hook = nameof(UpdateGender))] public int selectedGender;
    [SyncVar(hook = nameof(UpdateHair))] public int selectedHair;
    
    [SyncVar(hook = nameof(UpdateArmor))] public int selectedArmor;




    private CharacterRandomizer characterRandomizer;

    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
            LoadCustomization(); // ✅ Load data from PlayerPrefs on spawn
        }

        characterRandomizer = GetComponent<CharacterRandomizer>();
        ApplyCustomization(); // ✅ Ensure customization is applied on all clients
    }

    // ✅ Load saved customization from PlayerPrefs & send it to the server
    private void LoadCustomization()
    {
        int race = PlayerPrefs.GetInt("CharacterRace", 0);
        int gender = PlayerPrefs.GetInt("CharacterGender", 0);
        int hair = PlayerPrefs.GetInt("CharacterHair", 0);
        int armor = PlayerPrefs.GetInt("CharacterArmor", 0);

        Debug.Log($"🛠 Loaded PlayerPrefs: Race={race}, Gender={gender}, Hair={hair}, Armor={armor}");

        CmdSetCustomization(race, gender, hair, armor);
    }

    // ✅ Command: Send data from Client → Server
    [Command]
    public void CmdSetCustomization(int race, int gender, int hair, int armor)
    {
        selectedRace = race;
        selectedGender = gender;
        selectedHair = hair;
        selectedArmor = armor;

        Debug.Log($"✅ CmdSetCustomization: Race={race}, Gender={gender}, Hair={hair}, Armor={armor}");
    }

    // ✅ Hooks: These update the character when a SyncVar changes
    void UpdateRace(int oldRace, int newRace)
    {
        Debug.Log($"🎭 Race Updated: {newRace}");
        ApplyCustomization();
    }

    void UpdateGender(int oldGender, int newGender)
    {
        Debug.Log($"🚻 Gender Updated: {newGender}");
        ApplyCustomization();
    }

    void UpdateHair(int oldHair, int newHair)
    {
        Debug.Log($"💇 Hair Updated: {newHair}");
        ApplyHair(newHair); // ✅ Apply only hair update
    }

private void UpdateArmor(int oldArmor, int newArmor)
{
    Debug.Log($"🛡 Updating Full Armor Set: {oldArmor} → {newArmor}");
    ApplyFullArmor(newArmor);
}


    // ✅ Apply customization on all clients
    private void ApplyCustomization()
    {
        if (characterRandomizer == null)
        {
            Debug.LogError("❌ CharacterRandomizer is missing! Cannot apply customization.");
            return;
        }

        Debug.Log($"🎭 Applying Customization: Race={selectedRace}, Gender={selectedGender}, Hair={selectedHair}, Armor={selectedArmor}");

        characterRandomizer.SetCharacter(selectedRace, selectedGender, selectedHair, selectedArmor);

        // ✅ Ensure Elf ears are applied when the race is Elf
        ApplyElfFeatures(selectedRace);

        // ✅ Ensure the correct hair is applied
        ApplyHair(selectedHair);
    }

    // ✅ Ensure Elf ears are properly applied
    private void ApplyElfFeatures(int race)
    {
        if (characterRandomizer.allGender.elf_Ear.Count > 0)
        {
            if (race == 1) // 1 = Elf
            {
                Debug.Log("🧝 Applying Elf Ears...");
                foreach (var ear in characterRandomizer.allGender.elf_Ear)
                {
                    ear.SetActive(true);
                }
            }
            else
            {
                Debug.Log("👤 Hiding Elf Ears...");
                foreach (var ear in characterRandomizer.allGender.elf_Ear)
                {
                    ear.SetActive(false);
                }
            }
        }
    }

    [Command]
public void CmdSetArmor(int armorIndex)
{
    selectedArmor = armorIndex; // ✅ Store new armor selection
    Debug.Log($"🛡 Syncing Full Armor Set: {armorIndex}");
}



private void ApplyArmor(int armorIndex)
{
    // ✅ Disable all previous armor pieces before applying a new one
    foreach (var armorObj in characterRandomizer.allGender.chest_Attachment)
    {
        armorObj.SetActive(false);
    }

    // ✅ Apply correct armor based on gender
    if (selectedGender == 0) // Male
    {
        if (armorIndex < characterRandomizer.male.armor.Count)
        {
            Debug.Log($"🛡 Applying Male Armor: {armorIndex}");
            characterRandomizer.male.armor[armorIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ Male armor index out of range!");
        }
    }
    else // Female
    {
        if (armorIndex < characterRandomizer.female.armor.Count)
        {
            Debug.Log($"🛡 Applying Female Armor: {armorIndex}");
            characterRandomizer.female.armor[armorIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ Female armor index out of range!");
        }
    }
}


private void ApplyFullArmor(int armorIndex)
{
    // ✅ Disable all old armor pieces before applying a new set
    foreach (var armorObj in characterRandomizer.male.torso) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.arm_Upper_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.arm_Upper_Left) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.arm_Lower_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.arm_Lower_Left) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.hand_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.hand_Left) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.hips) armorObj.SetActive(false); // ✅ Hips
    foreach (var armorObj in characterRandomizer.male.leg_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.male.leg_Left) armorObj.SetActive(false);

    foreach (var armorObj in characterRandomizer.female.torso) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.arm_Upper_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.arm_Upper_Left) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.arm_Lower_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.arm_Lower_Left) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.hand_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.hand_Left) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.hips) armorObj.SetActive(false); // ✅ Hips
    foreach (var armorObj in characterRandomizer.female.leg_Right) armorObj.SetActive(false);
    foreach (var armorObj in characterRandomizer.female.leg_Left) armorObj.SetActive(false);

    // ✅ Apply correct armor based on gender
    if (selectedGender == 0) // Male
    {
        if (armorIndex < characterRandomizer.male.torso.Count)
            characterRandomizer.male.torso[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.arm_Upper_Right.Count)
            characterRandomizer.male.arm_Upper_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.arm_Upper_Left.Count)
            characterRandomizer.male.arm_Upper_Left[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.arm_Lower_Right.Count)
            characterRandomizer.male.arm_Lower_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.arm_Lower_Left.Count)
            characterRandomizer.male.arm_Lower_Left[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.hand_Right.Count)
            characterRandomizer.male.hand_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.hand_Left.Count)
            characterRandomizer.male.hand_Left[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.hips.Count) // ✅ Apply Hips
            characterRandomizer.male.hips[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.leg_Right.Count)
            characterRandomizer.male.leg_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.male.leg_Left.Count)
            characterRandomizer.male.leg_Left[armorIndex].SetActive(true);
    }
    else // Female
    {
        if (armorIndex < characterRandomizer.female.torso.Count)
            characterRandomizer.female.torso[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.arm_Upper_Right.Count)
            characterRandomizer.female.arm_Upper_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.arm_Upper_Left.Count)
            characterRandomizer.female.arm_Upper_Left[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.arm_Lower_Right.Count)
            characterRandomizer.female.arm_Lower_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.arm_Lower_Left.Count)
            characterRandomizer.female.arm_Lower_Left[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.hand_Right.Count)
            characterRandomizer.female.hand_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.hand_Left.Count)
            characterRandomizer.female.hand_Left[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.hips.Count) // ✅ Apply Hips
            characterRandomizer.female.hips[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.leg_Right.Count)
            characterRandomizer.female.leg_Right[armorIndex].SetActive(true);
        if (armorIndex < characterRandomizer.female.leg_Left.Count)
            characterRandomizer.female.leg_Left[armorIndex].SetActive(true);
    }

    Debug.Log($"✅ Full Armor Set (including hips) Applied: {armorIndex}");
}



    // ✅ Ensure the correct hair is applied
    private void ApplyHair(int hairIndex)
    {
        // Disable all previous hair options
        foreach (var hairObj in characterRandomizer.allGender.all_Hair)
        {
            hairObj.SetActive(false);
        }

        // Select the correct hair based on gender
        if (selectedGender == 0) // Male
        {
            if (hairIndex < characterRandomizer.male.hair.Count)
            {
                Debug.Log($"🧑 Applying Male Hair: {hairIndex}");
                characterRandomizer.male.hair[hairIndex].SetActive(true);
            }
            else
            {
                Debug.LogWarning("⚠️ Male hair index out of range!");
            }
        }
        else // Female
        {
            if (hairIndex < characterRandomizer.female.hair.Count)
            {
                Debug.Log($"👩 Applying Female Hair: {hairIndex}");
                characterRandomizer.female.hair[hairIndex].SetActive(true);
            }
            else
            {
                Debug.LogWarning("⚠️ Female hair index out of range!");
            }
        }
    }
}
