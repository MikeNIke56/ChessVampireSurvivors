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

    //each weapon's respective bullet object
    public GameObject bulletObj;

    private void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {

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
        GameObject bulletObjCopy = Instantiate<GameObject>(bulletObj, fireOffset.position, fireOffset.rotation);

        //sets the speed and damage of the bullet
        ProjectileBaseClass projectile = bulletObjCopy.GetComponent<ProjectileBaseClass>();
        projectile.SetDamage(attack);
        Vector3 force = transform.right * projectile.GetSpeed();
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

}
