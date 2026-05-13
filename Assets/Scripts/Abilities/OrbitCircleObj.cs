using Unity.VisualScripting;
using UnityEngine;

public class OrbitCircleObj : MonoBehaviour
{
    public float damage;
    public float knockBackAmnt;

    //the layers of objects this object is allowed to apply physics to
    public LayerMask targetLayers;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if the collided object is on a layer we should interact with...
        if (IsInLayerMask(collider.gameObject, targetLayers))
        {
            GameObject entity = collider.gameObject;

            //and if it implements the IDamageable interface
            if (entity.TryGetComponent(out IDamageable myInterface))
            {
                //cause damage and apply knockback
                entity.GetComponent<EntityBaseClass>().TakeDamage(damage);

                
            }
            //enemy projectile
            else
            {
                ProjectileBaseClass enemyBullet = 
                    entity.GetComponent<ProjectileBaseClass>();

                ObjectPoolingManager.ReturnObjectToPool(enemyBullet.gameObject,
                    ObjectPoolingManager.PoolType.Bullet);
            }    
        }
    }

    /**
     * checks if object's layermask matches the one being checked
     */
    protected bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
