using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move Info")]
    public float moveSpeed = 12f;

    private PlayerStateMachine stateMachine;
    public PlayerMoveState moveState;
    public PlayerIdleState idleState;

    private Animator animator;
    private Rigidbody2D body;

    public Animator Animator => animator;
    public Rigidbody2D Rigidbody => body;

    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = new PlayerStateMachine();

        moveState = new PlayerMoveState("Move", stateMachine, this);
        idleState = new PlayerIdleState("Idle", stateMachine, this);

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
