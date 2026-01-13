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

    public float despawnDistance = 20f;
    Transform player;

    void Awake()
    {
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currentMoveSpeed = enemyData.MoveSpeed;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            ReturnEnemy();
        }
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

    private void OnDestroy()
    {
        FindObjectOfType<EnemySpawner>().OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }
}
