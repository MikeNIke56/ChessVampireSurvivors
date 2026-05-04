using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* base class driver for all enemies
*/
public class EnemyBaseClass : EntityBaseClass
{
    public enum EnemyStates
    {
        Chasing,
        Shooting,
        Death,
        Dodging,
        CollisionAttackCooldown
    }

    [Header("Enemy Base Variables")]
    [SerializeField] protected List<EnemyStates> currentEnemyStates;
    [SerializeField] protected float attack;
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected float maxAttackCooldown;

    private float curAttackCooldown;


    //the layers of objects this object is allowed to apply physics to
    public LayerMask bodyCollisionTargetLayers;

    protected PlayerController player;

    private void Start()
    {

    }

    public virtual void Setup(PlayerController player)
    {
        curHealth = maxHealth;
        curAttackCooldown = maxAttackCooldown;
        this.player = player;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void RunBehavior()
    {
        if(currentEnemyStates.Contains(EnemyStates.CollisionAttackCooldown))
        {
            ReduceCollisionAttackCooldown();
        }   
    }

    public virtual void HandleMovement()
    {
    }

    protected void ChasePlayer()
    {
        if(player && rb)
        {
            LookAt(player.transform.position);
            rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * transform.right;
        }
    }

    public override void TakeDamage(float damage)
    {
        float calculatedDamage = Mathf.Clamp(damage - defense, 1, damage);
        curHealth -= calculatedDamage;

        if (curHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log(name + " died");
        Destroy(gameObject);
    }

    /**
     * collsion methods to check if player collided with us and if we are still
     * actively colliding
     */
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //if we're not on collision cooldown...
        if(!currentEnemyStates.Contains(EnemyStates.CollisionAttackCooldown))
        {
            //if the collided object is on a layer we should interact with...
            if (IsInLayerMask(collision.gameObject, bodyCollisionTargetLayers))
            {
                GameObject entity = collision.gameObject;

                //and if it implements the IDamageable interface
                if (entity.TryGetComponent(out IDamageable myInterface))
                {
                    //cause damage
                    entity.GetComponent<EntityBaseClass>().TakeDamage(attack);
                    currentEnemyStates.Add(EnemyStates.CollisionAttackCooldown);
                }

            }
        } 
    }
    protected void OnCollisionStay2D(Collision2D collision)
    {
        //if we're not on collision cooldown...
        if (!currentEnemyStates.Contains(EnemyStates.CollisionAttackCooldown))
        {
            //if the collided object is on a layer we should interact with...
            if (IsInLayerMask(collision.gameObject, bodyCollisionTargetLayers))
            {
                GameObject entity = collision.gameObject;

                //and if it implements the IDamageable interface
                if (entity.TryGetComponent(out IDamageable myInterface))
                {
                    //cause damage
                    entity.GetComponent<EntityBaseClass>().TakeDamage(attack);
                    currentEnemyStates.Add(EnemyStates.CollisionAttackCooldown);
                }

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

    /**
     * checks if object's layermask matches the one being checked
     */
    private void ReduceCollisionAttackCooldown()
    {
        curAttackCooldown -= Time.deltaTime;

        if (curAttackCooldown <= 0)
        {
            currentEnemyStates.Remove(EnemyStates.CollisionAttackCooldown);
            curAttackCooldown = maxAttackCooldown;
        }
    }
}
