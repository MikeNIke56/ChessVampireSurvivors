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
        CollisionAttackCooldown,
        ProjectileAttackCooldown,
        Retreating,
        Charging,
        ChargingAttackCooldown
    }

    [Header("Enemy Base Variables")]
    [SerializeField] protected List<EnemyStates> currentEnemyStates;
    [SerializeField] protected float collisionAttack;
    [SerializeField] protected float projectileAttack;
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected float maxCollisionAttackCooldown;
    [SerializeField] protected float maxProjectileCooldown;
    [SerializeField] protected int percentChanceToSpawn;
    [SerializeField] protected int lowerChance;
    [SerializeField] protected int upperChance;

    private float curCollisionAttackCooldown;
    private float curProjectileCooldown;


    //the layers of objects this object is allowed to apply physics to
    public LayerMask bodyCollisionTargetLayers;

    protected PlayerController player;

    public virtual void Setup(PlayerController player)
    {
        curHealth = maxHealth;
        curCollisionAttackCooldown = maxCollisionAttackCooldown;
        curProjectileCooldown = maxProjectileCooldown;
        this.player = player;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void RunBehavior()
    {
        if(currentEnemyStates.Contains(EnemyStates.CollisionAttackCooldown))
        {
            ReduceCollisionAttackCooldown();
        }
        if (currentEnemyStates.Contains(EnemyStates.ProjectileAttackCooldown))
        {
            ReduceProjectileAttackCooldown();
        }
    }

    public virtual void HandleMovement()
    {
    }

    protected virtual void ChasePlayer()
    {
        if(player && rb)
        {
            LookAt(player.transform.position, 180);
            rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * transform.right;
        }
    }

    protected virtual void Shoot()
    {
        
    }

    protected virtual void Shoot(Transform firepoint)
    {

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
                    entity.GetComponent<EntityBaseClass>().TakeDamage(collisionAttack);
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
                    entity.GetComponent<EntityBaseClass>().TakeDamage(collisionAttack);
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
     * prevents continuous collision detection
     */
    private void ReduceCollisionAttackCooldown()
    {
        curCollisionAttackCooldown -= Time.deltaTime;

        if (curCollisionAttackCooldown <= 0)
        {
            currentEnemyStates.Remove(EnemyStates.CollisionAttackCooldown);
            curCollisionAttackCooldown = maxCollisionAttackCooldown;
        }
    }

    /**
     * acts as the fire rate of enemies that shoot
     */
    protected void ReduceProjectileAttackCooldown()
    {
        curProjectileCooldown -= Time.deltaTime;

        if (curProjectileCooldown <= 0)
        {
            currentEnemyStates.Remove(EnemyStates.ProjectileAttackCooldown);
            curProjectileCooldown = maxProjectileCooldown;
        }
    }

    public int GetChanceToSpawn()
    {
        return percentChanceToSpawn;
    }
    public int GetLowerChance()
    {
        return lowerChance;
    }
    public int GetUpperChance()
    {
        return upperChance;
    }

    public void SetLowerChance(int chance)
    {
        this.lowerChance = chance;
    }
    public void SetUpperChance(int chance)
    {
        this.upperChance = chance;
    }
    public void SetPercentChanceToSpawn(int chance)
    {
        this.lowerChance = chance;
    }
}
