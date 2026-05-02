using UnityEngine;
using Terresquall;
using UnityEngine.InputSystem;

/**
* player driver script
*/
public class PlayerController : EntityBaseClass
{
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

        //load in and equip our weapon
        GameObject primaryWeaponGameObjectCopy = Instantiate<GameObject>(primaryWeaponGameObject, 
            weaponPivotPoint.transform);

        primaryWeapon = primaryWeaponGameObjectCopy.GetComponent<WeaponBaseClass>();
        primaryWeapon.SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        moveVal = moveAction.ReadValue<Vector2>();

        if (shootAction.WasPressedThisFrame())
            Shoot();
        if (dodgeAction.WasPressedThisFrame() && !currentStates.Contains(EntityState.Dodging))
            Dodge();

        Look();
    }

    void FixedUpdate()
    {
        //inputVelocity is our current move direction * our speed
        Vector2 inputVelocity = moveVal * moveSpeed;

        if (currentStates.Contains(EntityState.Dodging))
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
            currentStates.Remove(EntityState.Dodging);
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
        if (!currentStates.Contains(EntityState.Dodging))
        {
            currentStates.Add(EntityState.Dodging);
            rb.linearVelocity = moveVal * dodgePower;
        }
    }


    public void Shoot()
    {
        if (primaryWeapon)
            primaryWeapon.Fire();
    }
}
