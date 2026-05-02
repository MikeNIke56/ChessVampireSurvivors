using UnityEngine;

/**
* base class for all projectile bullets
*/
public class ProjectileBaseClass : MonoBehaviour
{
    [Header("Base Variables")]
    protected Rigidbody2D rb;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;

    //the layers of objects this object is allowed to apply physics to
    public LayerMask targetLayers;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject, targetLayers))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    /*
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
}
