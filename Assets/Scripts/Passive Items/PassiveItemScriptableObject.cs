using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/PassiveItem")]
public class PassiveItemScriptableObject : ScriptableObject
{

    [SerializeField]
    float multipler;
    public float Multipler { get=> multipler; private set=> multipler = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    int level; // Not meant to be modified in the game [Only in Editor]
    public int Level{get => level; private set => level = value;}

    [SerializeField]
    GameObject nextLevelPrefab; // The perfab of the next level i.e. what the object becomes when it levels up
                                // Not to  be confused with the prefab to be spawned at the next level
    public GameObject NextLevelPrefab{get => nextLevelPrefab; private set => nextLevelPrefab = value;}

    [SerializeField]
    new string name;
    public string Name{get => name; private set => name = value;}

    [SerializeField]
    string description; // What is the description of this weapon? [if this weapon is an upgrade, place the description of the upgrades]
    public string Description{get => description; private set => description = value;}

    [SerializeField]
    Sprite icon;
    public Sprite Icon{get => icon; private set => icon = value;}
}
