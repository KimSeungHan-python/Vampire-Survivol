using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Current Stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionsChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirx < 0 && diry == 0)//left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if(dirx == 0 && diry < 0)//down
        {
            rotation.z = 90f;
        }
        else if(dirx == 0 && diry > 0)//up
        {
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if(dirx > 0 && diry > 0)//up-right
        {
            scale.y = scale.y * -1;
            rotation.z = -45f;
        }
        else if(dirx < 0 && diry > 0)//up-left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -135f;
        }
        else if(dirx > 0 && diry < 0)//down-right
        {
            rotation.z = 45f;
        }
        else if(dirx < 0 && diry < 0)//down-left
        {
            rotation.z = -45f;
        }   
        

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyStats>().TakeDamage(GetCurrentDamage(), transform.position);
            ReducePierce();
        }
        else if(collision.CompareTag("Prop"))
        {
            if(collision.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }

    }

    void ReducePierce()
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }

}
