using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Animations;
public class Pickup : MonoBehaviour
{
    //public bool hasBeenCollected = false;
    public float lifespan = 0.5f;
    protected PlayerStats target; // If the pickup has a target, then fly towards the target.
    protected float speed; // The speed at which the pickup travels.
    Vector2 initialPosition;
    float initialOffset;

    // To represent the bobbing animation of the object.
    [System.Serializable]
    public struct BobbingAnimation
    {
        public float frequency;
        public Vector2 direction;
    }
    public BobbingAnimation bobbingAnimation = new BobbingAnimation
    {
        frequency = 2f,
        direction = new Vector2(0, 0.3f)
    };

    [Header("Bonuses")]
    public int experience;
    public int health;

    protected virtual void Start()
    {
        initialPosition = transform.position;
        initialOffset = Random.Range(0f,bobbingAnimation.frequency);
    }

    protected virtual void Update()
    {
        if (target)
        {
            // Move if towards the player and check thhe distance between.
            Vector2 distance = target.transform.position - transform.position;
            if (distance.sqrMagnitude > speed * speed * Time.deltaTime)
            {
                transform.position += (Vector3)distance.normalized * speed * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position = initialPosition + bobbingAnimation.direction * Mathf.Sin((Time.time + initialOffset) * bobbingAnimation.frequency);
        }

    }

    public virtual bool Collect(PlayerStats target, float speed, float lifespan = 0f)
    {
        // hasBeenCollected = true;
        if (!this.target)
        {
            this.target =target;
            this.speed = speed;
            if (lifespan > 0)
            {
                this.lifespan = lifespan;
            }
            Destroy(gameObject, Mathf.Max(0.01f, this.lifespan));
            return true;
        }
        else
        {
            // Handle the animation of the object.
            transform.position = initialPosition + bobbingAnimation.direction
                * Mathf.Sin(Time.time * bobbingAnimation.frequency);
        }
        return false;
    }

    protected virtual void OnDestroy()
    {
        if (!target) return;
        if (experience != 0)
        {
            target.IncreaseExperience(experience);
        }
        if (health != 0)
        {
            target.RestoreHealth(health);
        }
    }
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if(collision.gameObject.CompareTag("Player"))
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
