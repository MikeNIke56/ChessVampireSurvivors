using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* base class for all entities
*/
public class EntityBaseClass : MonoBehaviour, IDamageable
{
    [Header("Base Variables")]
    [SerializeField] protected float defense;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float moveSpeed;

    protected float curHealth;
    protected Rigidbody2D rb;


    protected void LookAt(Vector3 target)
    {
        //find angle between the player and target
        float lookAngle = AngleBetweenTwoPoints(transform.position, target) + 180;

        //apply target rotation on the z axis
        transform.eulerAngles = new Vector3(0, 0, lookAngle);
    }

    private float AngleBetweenTwoPoints(Vector3 point1,  Vector3 point2)
    {
        return Mathf.Atan2(point1.y - point2.y, point1.x - point2.x) * Mathf.Rad2Deg;
    }

    public float GetHealth()
    {
        return curHealth;
    }

    public virtual void TakeDamage(float damage)
    {

    }

    public virtual void Die()
    {

    }
}

/**
 * the interface for entities to take damage
 */
public interface IDamageable
{
    void TakeDamage(float damage);
}
