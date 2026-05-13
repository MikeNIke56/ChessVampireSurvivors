using UnityEngine;

public class Sniper : WeaponBaseClass
{
    [SerializeField] private int newPenetrationValue;
    [SerializeField] private int newBulletSpeed;

    public override void UpgradeWeapon(int level)
    {
        base.UpgradeWeapon(level);
        newPenetrationValue += level;
        newBulletSpeed += level;
    }

    public override void Fire()
    {
        //loads in and fires bullet
        GameObject bulletObjCopy = ObjectPoolingManager.SpawnObject(bulletObj, fireOffset.position,
            fireOffset.rotation, ObjectPoolingManager.PoolType.Bullet);

        //sets the speed and damage of the bullet
        ProjectileBaseClass projectile = bulletObjCopy.GetComponent<ProjectileBaseClass>();
        projectile.SetDamage(attack);
        projectile.penetrationValue = newPenetrationValue;
        projectile.SetSpeed(newBulletSpeed);

        //grab the bullet cone of the weapon and set the bullet's random
        //direction
        float spread = Random.Range(-bulletSpread, bulletSpread);
        Vector3 direction = transform.right + transform.up * spread;

        //keep consistent speed
        direction.Normalize();

        Vector3 force = direction * projectile.GetSpeed();
        projectile.GetRigidbody().AddForce(force, ForceMode2D.Impulse);
    }
}
