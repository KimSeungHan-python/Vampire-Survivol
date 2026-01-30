using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using UnityEngine;


// // <summary>
// // Component that you attach to all projectile perfabs. Al spawned projectiles will fly in the direction
// // they are facing and deal damage when they hit an object.
// // </summary>
public class Projectile : WeaponEffect
{
    public enum DamageSource { Projectile, owner };
    public DamageSource damageSource = DamageSource.Projectile;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed =  new Vector3(0,0,0);

    protected Rigidbody2D rb;
    protected int piercing;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Weapon.Stats stats = weapon.GetStats();
        if(rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.velocity = transform.up * stats.speed;
        }

        // Prevent the area from being 0, as it hides the projectile
        float area = stats.area == 0 ? 1 : stats.area;
        transform.localScale = new Vector3(area * Mathf.Sign(transform.localScale.x), 
        area * Mathf.Sign(transform.localScale.y));

        // Set how much piercing this object has.
        piercing = stats.piercing;
        
        //Destroy the projectile after its lifespan expires
        if(stats.lifespan > 0)
            Destroy(gameObject, stats.lifespan);

        // If the projectile is auto-aiming, automatically find a suitable enemy
        if(hasAutoAim) AcquireAutoAimFacing();
    }

    public virtual void AcquireAutoAimFacing()
    {
        float aimAngle; // We need to determine where to aim.

        //Find all enemies on the screen.
        EnemyStats[] targets = FindObjectOfType<EnemyStats>();

        // Select a random enemy (if there is at least 1)
        // Otherwise, pick a random angle
        if(targets.Length > 0)
        {
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 difference = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else
        {
            aimAngle = Random.Range(0f, 360f);  
        }

        // Point the projectile towards where we are aiming at.
        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);
    }

    //Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //Only drive movement ourselves if this is a kinematic.
        if(rb.bodyType == RigidbodyType2D.Kinematic)
        {
            Weapon.Stats stats = weapon.GetStats();
            transform.position += transform.up * stats.speed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats es = other.GetComponent<EnemyStats>();
        BreakableProps p = other.GetComponent<BreakableProps>();

        // Only collide with enemies or breakable stuffs
        if(es)
    }
}
