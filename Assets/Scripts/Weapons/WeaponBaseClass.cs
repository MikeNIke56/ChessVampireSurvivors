using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseClass : MonoBehaviour
{
    [Header("Base Variables")]
    protected Rigidbody2D rb;
    [SerializeField] protected float attack;
    [SerializeField] protected float attackSpeed;
}
