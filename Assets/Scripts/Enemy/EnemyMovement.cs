using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    EnemyStats enemy;

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
    }
}
