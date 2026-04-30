using UnityEngine;
using Terresquall;
using UnityEngine.InputSystem;

public class PlayerController : EntityBaseClass
{
    //input Action Variables
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction shootAction;
    private InputAction dodgeAction;

    private Vector2 moveVal;

    [Header("Dodge Variables")]
    public float dodgePower;
    public float dodgeDecaySpeed;


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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVal = moveAction.ReadValue<Vector2>();

        if (shootAction.WasPressedThisFrame())
        {
            Shoot();
        }
        if (dodgeAction.WasPressedThisFrame() && !currentStates.Contains(EntityState.Dodging))
        {
            Dodge();
        }
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

    public void Dodge()
    {
        if (!currentStates.Contains(EntityState.Dodging))
        {
            currentStates.Add(EntityState.Dodging);
            //Vector2 direction = rb.linearVelocity.normalized;
            //Debug.DrawRay(rb.position, direction, Color.red, 2f);
            rb.linearVelocity = moveVal * dodgePower;
            Debug.Log("dodge");
        }
    }


    public void Shoot()
    {
        Debug.Log("shoot");
    }
}
