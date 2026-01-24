using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest()
    {
        WeaponEvolutionBluePrint toEvolve = inventoryManager.GetPossibleEvolutions()[Random.Range(0, inventoryManager.GetPossibleEvolutions().Count)];
        inventoryManager.EvolveWeapon(toEvolve);
    }


}
