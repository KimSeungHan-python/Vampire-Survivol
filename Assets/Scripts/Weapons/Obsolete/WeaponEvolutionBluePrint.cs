using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEvolutionBluePrint", menuName = "ScriptableObjects/WeaponEvolutionBluePrint")]
public class WeaponEvolutionBluePrint : ScriptableObject
{
    public WeaponScriptableObject baseWeaponData;
    public PassiveItemScriptableObject catalystPassiveItemData;
    public WeaponScriptableObject evolvedWeaponData;
    public GameObject evolvedWeapon;
}
