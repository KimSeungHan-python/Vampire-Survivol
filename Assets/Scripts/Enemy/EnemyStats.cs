using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EnemyStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EnemyScriptableObject enemyData;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]   
    public float currentDamage;
    [HideInInspector]
    public float currentMoveSpeed;

    void Awake()
    {
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currentMoveSpeed = enemyData.MoveSpeed;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    void Kill()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(currentDamage);
            //추가 효과들 여기에
        }
    }
}
