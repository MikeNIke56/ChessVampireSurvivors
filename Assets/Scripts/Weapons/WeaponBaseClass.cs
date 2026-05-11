using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

/**
* base class for all weapons
*/
public class WeaponBaseClass : MonoBehaviour
{
    [Header("Weapon Variables")]
    protected Rigidbody2D rb;
    [SerializeField] protected float attack;
    public float fireCooldown;
    [SerializeField] protected float primaryWeaponSpawnOffset;
    [SerializeField] protected Transform fireOffset;
    public bool canFire = true;
    [SerializeField] protected float bulletCone;

    //each weapon's respective bullet object
    public GameObject bulletObj;

    private void Start()
    {
        SetUp();
    }

    /**
    * all initializations to run once a weapon is created
    */
    public void SetUp()
    {
        rb = GetComponent<Rigidbody2D>();
        SetWeaponPivotOffset();
    }

    public virtual void Fire()
    {
        //loads in and fires bullet
        GameObject bulletObjCopy = ObjectPoolingManager.SpawnObject(bulletObj, fireOffset.position,
            fireOffset.rotation, ObjectPoolingManager.PoolType.Bullet);

        //sets the speed and damage of the bullet
        ProjectileBaseClass projectile = bulletObjCopy.GetComponent<ProjectileBaseClass>();
        projectile.SetDamage(attack);

        //grab the bullet cone of the weapon and set the bullet's random
        //direction
        float spread = Random.Range(-bulletCone, bulletCone);
        Vector3 direction = transform.right + transform.up * spread;

        //keep consistent speed
        direction.Normalize();

        Vector3 force = direction * projectile.GetSpeed();
        projectile.GetRigidbody().AddForce(force, ForceMode2D.Impulse);
    }

    /**
    * sets how far the gun will be from the player
    */
    public void SetWeaponPivotOffset()
    {
        Vector3 newPos = transform.position;
        newPos.x = primaryWeaponSpawnOffset; 
        transform.position = newPos;
    }

    public float GetWeaponFireCooldown()
    {
        return fireCooldown;
    }
    public float GetWeaponAttack()
    {
        return attack;
    }

    public void SetWeaponCooldown(float cooldown)
    {
        fireCooldown = cooldown;
    }
    public void SetWeaponAttack(float damage)
    {
        attack = damage;
    }
}
