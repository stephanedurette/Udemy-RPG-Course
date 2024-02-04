using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 8f;

    [Header("Ground Collision Info")]
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private Vector2 groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [Header("Wall Collision Info")]
    [SerializeField] private Transform wallCheckPosition;
    [SerializeField] private Vector2 wallCheckDistance;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    private PlayerStateMachine stateMachine;
    public PlayerMoveState moveState;
    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerFallState fallState;
    public PlayerDashState dashState;

    private Animator animator;
    private Rigidbody2D body;

    public Animator Animator => animator;
    public Rigidbody2D Rigidbody => body;

    public int facingDirection { get; private set; } = 1;

    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = new PlayerStateMachine();

        moveState = new PlayerMoveState("OnGround", stateMachine, this);
        idleState = new PlayerIdleState("OnGround", stateMachine, this);
        jumpState = new PlayerJumpState("InAir", stateMachine, this);
        fallState = new PlayerFallState("InAir", stateMachine, this);
        dashState = new PlayerDashState("Dash", stateMachine, this);

        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.CurrentState.Update();
    }

    public void SetVelocity(float velX, float velY)
    {
        body.velocity = new Vector2(velX, velY);

        if (body.velocity.x != 0)
            SetDirection(body.velocity.x > 0 ? 1 : -1);
    }

    public bool IsOnGround() => Physics2D.Raycast(groundCheckPosition.position, groundCheckDistance.normalized, groundCheckDistance.magnitude, groundLayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckPosition.position, groundCheckPosition.position + (Vector3)groundCheckDistance);
        Gizmos.DrawLine(wallCheckPosition.position, wallCheckPosition.position + (Vector3)wallCheckDistance);
    }

    public void SetDirection(int direction)
    {
        this.facingDirection = direction;
        this.transform.rotation = Quaternion.Euler(0, direction == 1 ? 0 : 180, 0);
    }
}
