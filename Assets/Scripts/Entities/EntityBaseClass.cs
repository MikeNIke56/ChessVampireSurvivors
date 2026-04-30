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
}
