using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 8f;

    private PlayerStateMachine stateMachine;
    public PlayerMoveState moveState;
    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerFallState fallState;

    private Animator animator;
    private Rigidbody2D body;

    public Animator Animator => animator;
    public Rigidbody2D Rigidbody => body;

    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = new PlayerStateMachine();

        moveState = new PlayerMoveState("OnGround", stateMachine, this);
        idleState = new PlayerIdleState("OnGround", stateMachine, this);
        jumpState = new PlayerJumpState("InAir", stateMachine, this);
        fallState = new PlayerFallState("InAir", stateMachine, this);

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
        body.velocity = new Vector2 (velX, velY);
    }
}
