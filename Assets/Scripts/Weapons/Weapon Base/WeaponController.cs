using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
//basic weapon controller script
public class WeaponController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Weapon Stats")]
    public GameObject prefabs;
    public float damage;
    public float speed;
    public float cooldownDuration;
    float currentCooldown;
    public int pierce;

    protected PlayerMovement pm;
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = cooldownDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();
        }    
    }

    protected virtual void Attack()
    {
        //Instantiate(prefabs, transform.position, transform.rotation);
        currentCooldown = cooldownDuration;
    }   
}
