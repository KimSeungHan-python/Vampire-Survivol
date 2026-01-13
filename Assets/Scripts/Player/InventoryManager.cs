using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
using UnityEngine.UI;
using System.Reflection;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponSlotsUI = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemSlotsUI = new List<Image>(6);

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] =weapon.weaponData.Level;
        weaponSlotsUI[slotIndex].enabled = true ;
        weaponSlotsUI[slotIndex].sprite = weapon.weaponData.Icon;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemSlotsUI[slotIndex].enabled = true ;
        passiveItemSlotsUI[slotIndex].sprite = passiveItem.passiveItemData.Icon;
    }

    public void LevelUpWeapon(int slotIndex)
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
        }

    }

    public void LevelUpPassiveItem(int slotIndex)
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
        }
    }
}
