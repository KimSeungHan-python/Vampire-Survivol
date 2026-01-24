using UnityEngine;
using System.Collections.Generic;
using System.Collections;



public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(knockbackDuration > 0)
        {
            transform.position += (Vector3)(knockbackVelocity * Time.deltaTime);
            knockbackDuration -= Time.deltaTime;
            return;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);

        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        //Ignore the knockback if the duration is greater than 0
        if(knockbackDuration > 0) return;

        // Begins the knockback effect
        knockbackVelocity = velocity;
        knockbackDuration = duration;

    }
}
