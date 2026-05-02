using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseClass : MonoBehaviour
{
    public enum EntityState
    {
        Walking,
        Shooting,
        Death,
        Dodging,
        OnCooldown
    }

    [Header("Base Variables")]
    [SerializeField] protected List<EntityState> currentStates;
    protected Rigidbody2D rb;
    [SerializeField] protected float attack;
    [SerializeField] protected float defense;
    private float curHealth;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackSpeed;

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
}
