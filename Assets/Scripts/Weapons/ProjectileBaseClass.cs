using UnityEngine;

/**
* base class for all projectile bullets
*/
public class ProjectileBaseClass : MonoBehaviour
{
    [Header("Base Variables")]
    protected Rigidbody2D rb;
    private float damage;
    [SerializeField] protected float speed;

    //the layers of objects this object is allowed to apply physics to
    public LayerMask targetLayers;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //if the collided object is on a layer we should interact with...
        if (IsInLayerMask(collision.gameObject, targetLayers))
        {
            GameObject entity = collision.gameObject;

            //and if it implements the IDamageable interface
            if (entity.TryGetComponent(out IDamageable myInterface))
                //cause damage
                entity.GetComponent<EntityBaseClass>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }

    /**
     * checks if object's layermask matches the one being checked
     */
    protected bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
