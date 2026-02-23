using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponSlotsUI = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemSlotsUI = new List<Image>(6);

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }
    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public TextMeshProUGUI upgradeNameDisplay;
        public TextMeshProUGUI upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    public List<WeaponEvolutionBluePrint> weaponEvolutions = new List<WeaponEvolutionBluePrint>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] =weapon.weaponData.Level;
        weaponSlotsUI[slotIndex].enabled = true ;
        weaponSlotsUI[slotIndex].sprite = weapon.weaponData.Icon;

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUP();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemSlotsUI[slotIndex].enabled = true ;
        passiveItemSlotsUI[slotIndex].sprite = passiveItem.passiveItemData.Icon;

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUP();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if(weaponSlots.Count >slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if(!weapon.weaponData.NextLevelPrefab)
            {
                Debug.Log("Weapon is at max level" + weapon.name);
                return;
            }
            GameObject upgradeWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradeWeapon.transform.SetParent(transform); // Set the weapon as a child of the player
            AddWeapon(slotIndex, upgradeWeapon.GetComponent<WeaponController>());//Add the weapon th it's inventory slot
            Destroy(weapon.gameObject); 
            weaponLevels[slotIndex] = upgradeWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradeOptions[upgradeIndex].weaponData = upgradeWeapon.GetComponent<WeaponController>().weaponData;
            if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUP();
            }
        }

    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if(passiveItemSlots.Count >slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if(!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.Log("Passive Item is at max level" + passiveItem.name);
                return;
            }
            GameObject upgradePassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradePassiveItem.transform.SetParent(transform); // Set the weapon as a child of the player
            AddPassiveItem(slotIndex, upgradePassiveItem.GetComponent<PassiveItem>());//Add the weapon th it's inventory slot
            Destroy(passiveItem.gameObject); 
            passiveItemLevels[slotIndex] = upgradePassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;

            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = upgradePassiveItem.GetComponent<PassiveItem>().passiveItemData;
            if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUP();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);    

        foreach(var upgradeOption in upgradeUIOptions)
        {
            if(availableWeaponUpgrades.Count ==0 && availablePassiveItemUpgrades.Count ==0)
            {
                Debug.Log("No more upgrades available");
                return;
            }

            int upgradeType;
            if(availableWeaponUpgrades.Count ==0)
            {
                upgradeType =2; //Passive Item
            }
            else if(availablePassiveItemUpgrades.Count ==0)
            {
                upgradeType =1; //Weapon
            }
            else
            {
                upgradeType = Random.Range(1,3); //1: Weapon, 2: Passive Item
            }
            if(upgradeType ==1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);
                if(chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = false;
                    for(int i =0; i <weaponSlots.Count; i++)
                    {
                        if(weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if(!newWeapon)
                            {
                                if(!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    Debug.Log("Weapon is at max level" + chosenWeaponUpgrade.weaponData.Name);
                                    break;
                                }
                                
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex));
                                //Set the description and name to be that of the next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if(newWeapon)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        //Apply initial description and name
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if(upgradeType ==2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if(chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newPassiveItem = false;
                    for(int i =0; i <passiveItemSlots.Count; i++)
                    {
                        if(passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;
                            if(!newPassiveItem)
                            {
                                if(!chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    Debug.Log("Passive Item is at max level" + chosenPassiveItemUpgrade.passiveItemData.Name);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex));
                                 //Set the description and name to be that of the next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if(newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        //Apply initial description and name
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach(var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgradeOptions()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    public List<WeaponEvolutionBluePrint> GetPossibleEvolutions()
    {
        List<WeaponEvolutionBluePrint> possibleEvolutions = new List<WeaponEvolutionBluePrint>();

        foreach(WeaponController weapon in weaponSlots)
        {
            if(weapon != null)
            {
                foreach(PassiveItem catalyst in passiveItemSlots)
                {
                    if(catalyst != null)
                    {
                        foreach(WeaponEvolutionBluePrint evolution in weaponEvolutions)
                        {
                            if(weapon.weaponData.Level >= evolution.baseWeaponData.Level && catalyst.passiveItemData.Level >= evolution.catalystPassiveItemData.Level)
                            {
                                possibleEvolutions.Add(evolution);
                            }
                        }
                    }
                }
            }
        } 
        return possibleEvolutions;
    }
    

    public void EvolveWeapon(WeaponEvolutionBluePrint evolution)
    {
        for(int weaponSlotIndex = 0; weaponSlotIndex < weaponSlots.Count; weaponSlotIndex++)
        {
            WeaponController weapon = weaponSlots[weaponSlotIndex];
            
            for(int catalystSlotIndex = 0; catalystSlotIndex < passiveItemSlots.Count; catalystSlotIndex++)
            {
                PassiveItem catalyst = passiveItemSlots[catalystSlotIndex];

                if(weapon != null && catalyst != null)
                {
                    if(weapon.weaponData.Level >= evolution.baseWeaponData.Level && catalyst.passiveItemData.Level >= evolution.catalystPassiveItemData.Level)
                    {
                        //Evolve the weapon
                        GameObject evolvedWeapon = Instantiate(evolution.evolvedWeapon, transform.position, Quaternion.identity);
                        WeaponController evolvedWeaponController = evolvedWeapon.GetComponent<WeaponController>();
                        evolvedWeapon.transform.SetParent(transform); // Set the weapon as a child of the player
                        AddWeapon(weaponSlotIndex, evolvedWeaponController);//Add the weapon th it's inventory slot
                        Destroy(weapon.gameObject); 
                        
                        //Update level and icon UI
                        weaponLevels[weaponSlotIndex] = evolvedWeaponController.weaponData.Level;
                        weaponSlotsUI[weaponSlotIndex].sprite = evolvedWeaponController.weaponData.Icon;

                        //Update the upgrade options
                        weaponUpgradeOptions.RemoveAt(evolvedWeaponController.weaponData.EvolvedUpgradeToRemove);

                        Debug.Log("Evolved Weapon: " + evolvedWeaponController.weaponData.Name);

                        return;

                        
                    }
                }
            }
        }
    }
}

