using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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


}
