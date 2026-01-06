using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed{get => moveSpeed; private set => moveSpeed = value;}
    [SerializeField]
    float maxHealth;
    public float MaxHealth{get => maxHealth; private set => maxHealth = value;}
    [SerializeField]
    float damage;
    public float Damage{get => damage; private set => damage = value;}
}
