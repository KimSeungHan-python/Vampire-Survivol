using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GarlicBehaviour : MeleeWeaponBehaviour
{

    List<GameObject> markedEnemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !markedEnemies.Contains(collision.gameObject))
        {
            collision.GetComponent<EnemyStats>().TakeDamage(GetCurrentDamage());
            markedEnemies.Add(collision.gameObject);// 추가적인 피해 방지를 위해
        
            //추가 효과들 여기에
        }

        else if(collision.CompareTag("Prop"))
        {
            if(collision.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(collision.gameObject);// 추가적인 피해 방지를 위해
            }
        }
    }

    // Update is called once per frame

}
