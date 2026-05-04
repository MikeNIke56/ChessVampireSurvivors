using UnityEngine;
using Terresquall;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

/**
* player driver script
*/
public class PlayerController : EntityBaseClass
{
    public enum PlayerStates
    {
        Walking,
        Shooting,
        Death,
        Dodging
    }
    [SerializeField] protected List<PlayerStates> currentPlayerStates;


    [SerializeField] protected CircleCollider2D hitbox;

    //input Action Variables
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction shootAction;
    private InputAction dodgeAction;
    private InputAction lookAction;

    //input vector
    private Vector2 moveVal;

    [Header("Dodge Variables")]
    public float dodgePower;
    public float dodgeDecaySpeed;
    private float dodgeHitboxSize = .25f;
    private float baseHitboxSize;

    //player's current primary weapon
    private WeaponBaseClass primaryWeapon;
    public GameObject primaryWeaponGameObject;

    //primary weapon pivot
    public WeaponPivotPoint weaponPivotPoint;


    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        shootAction = InputSystem.actions.FindAction("Shoot");
        dodgeAction = InputSystem.actions.FindAction("Dodge");
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseHitboxSize = hitbox.radius;
        curHealth = maxHealth;

        //load in and equip our weapon
        GameObject primaryWeaponGameObjectCopy = Instantiate<GameObject>(primaryWeaponGameObject, 
            weaponPivotPoint.transform);

        primaryWeapon = primaryWeaponGameObjectCopy.GetComponent<WeaponBaseClass>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVal = moveAction.ReadValue<Vector2>();

        if (moveVal.sqrMagnitude > 0 && !currentPlayerStates.Contains(PlayerStates.Walking))
            currentPlayerStates.Add(PlayerStates.Walking);
        else if(moveVal.sqrMagnitude < .001f && currentPlayerStates.Contains(PlayerStates.Walking))
            currentPlayerStates.Remove(PlayerStates.Walking);

        if (shootAction.WasPressedThisFrame())
            Shoot();
        if (dodgeAction.WasPressedThisFrame() && !currentPlayerStates.Contains(PlayerStates.Dodging) &&
            moveVal.sqrMagnitude > 0)
            Dodge();

        Look();
    }

    void FixedUpdate()
    {
        //inputVelocity is our current move direction * our speed
        Vector2 inputVelocity = moveVal * moveSpeed;

        if (currentPlayerStates.Contains(PlayerStates.Dodging))
        {
            //we take the magnitude of our current velocity squared, not squaring is more expensive
            float sqrMagnitude = rb.linearVelocity.sqrMagnitude;

            //as long as the current velocity's magnitude is higher than movement's magnitude
            //and we are still moving ( > 1 )
            if (sqrMagnitude > inputVelocity.sqrMagnitude && sqrMagnitude > 1f)
            {
                //gradually slow down our velocity
                rb.linearVelocity -= dodgeDecaySpeed * Time.fixedDeltaTime * rb.linearVelocity.normalized;
                return;
            }
            currentPlayerStates.Remove(PlayerStates.Dodging);
            hitbox.radius = baseHitboxSize;
        }
        Move(inputVelocity);
    }

    public void Move(Vector2 inputVelocity)
    {
        rb.linearVelocity = inputVelocity;
    }

    public void Look()
    {
        //get the current mouse position in screen pixels
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (weaponPivotPoint)
            weaponPivotPoint.LookAtMouse(mousePosition);
    }

    public void Dodge()
    {
        if (!currentPlayerStates.Contains(PlayerStates.Dodging))
        {
            currentPlayerStates.Add(PlayerStates.Dodging);
            hitbox.radius = dodgeHitboxSize;
            rb.linearVelocity = moveVal * dodgePower;
        }
    }

    public void Shoot()
    {
        if (primaryWeapon && primaryWeapon.canFire == true)
        {
            primaryWeapon.Fire();
            StartCoroutine(StartFireCooldown());
        }
    }

    private IEnumerator StartFireCooldown()
    {
        currentPlayerStates.Add(PlayerStates.Shooting);
        primaryWeapon.canFire = false;

        yield return new WaitForSecondsRealtime(primaryWeapon.fireCooldown);

        primaryWeapon.canFire = true;
        currentPlayerStates.Remove(PlayerStates.Shooting);
    }

    public override void TakeDamage(float damage)
    {
        float calculatedDamage = Mathf.Clamp(damage - defense, 1, damage);
        curHealth -= calculatedDamage;
        Debug.Log(curHealth);

        if (curHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("player died");
    }
}
