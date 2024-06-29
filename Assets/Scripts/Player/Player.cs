using ImprovedTimers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform rotatePivot;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private InputReader inputReader;

    [Header("Collision Check References")]
    [SerializeField] private BoxCaster groundChecker;
    [SerializeField] private BoxCaster rightWallChecker;
    [SerializeField] private BoxCaster leftWallChecker;

    [Header("Move Settings")]
    public float moveSpeed = 12f;
    public float jumpForce = 8f;

    [Header("Air Move Settings")]
    public float maxAirVelocity = 20f;
    public float airAccel = 1f;

    public static Player Instance;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    private StateMachine stateMachine;

    //timers
    public CountdownTimer dashDurationTimer;
    public CountdownTimer coyoteTimer;
    CountdownTimer dashCooldownTimer;

    private float startingGravityScale;

    IState movingState, jumpingState, fallingState, wallslidingState, dashingState, attackingState;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        SetupTimers();
        SetupStateMachine();
    }

    private void SetupTimers()
    {
        dashDurationTimer = new CountdownTimer(dashDuration);
        dashCooldownTimer = new CountdownTimer(dashCooldown);
        coyoteTimer = new CountdownTimer(0.2f);

        dashDurationTimer.OnTimerStop += () => dashCooldownTimer.Start();
    }

    private void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        movingState = new PlayerMovementState(this);
        jumpingState = new PlayerJumpingState(this);
        fallingState = new PlayerFallingState(this);
        wallslidingState = new PlayerWallslidingState(this);
        dashingState = new PlayerDashingState(this);
        attackingState = new PlayerAttackingState(this);

        //Jump
        stateMachine.AddTransition(movingState, jumpingState, new FuncPredicate(() => inputReader.Jumping && groundChecker.IsColliding));
        stateMachine.AddTransition(wallslidingState, jumpingState, new FuncPredicate(() => inputReader.Jumping));
        stateMachine.AddTransition(fallingState, jumpingState, new FuncPredicate(() => inputReader.Jumping && coyoteTimer.IsRunning));

        //Fall
        stateMachine.AddTransition(jumpingState, fallingState, new FuncPredicate(() => rigidBody.velocity.y < 0));
        stateMachine.AddTransition(movingState, fallingState, new FuncPredicate(() => rigidBody.velocity.y < 0 && !groundChecker.IsColliding));
        stateMachine.AddTransition(wallslidingState, fallingState, new FuncPredicate(() => inputReader.MoveDirection.x == -WallDirection()));
        stateMachine.AddTransition(dashingState, fallingState, new FuncPredicate(() => !dashDurationTimer.IsRunning));

        //Wallslide
        stateMachine.AddTransition(fallingState, wallslidingState, new FuncPredicate(() => IsCollidingWithWall() && inputReader.MoveDirection.x != -WallDirection()));

        //Moving
        stateMachine.AddTransition(fallingState, movingState, new FuncPredicate(() => groundChecker.IsColliding));
        stateMachine.AddTransition(wallslidingState, movingState, new FuncPredicate(() => groundChecker.IsColliding));
        stateMachine.AddTransition(attackingState, movingState, new FuncPredicate(() => !attackManager.IsAttacking));

        //Dashing
        stateMachine.AddAnyTransition(dashingState, new FuncPredicate(() => inputReader.Dashing && !dashDurationTimer.IsRunning && !dashCooldownTimer.IsRunning));

        //Attacking
        stateMachine.AddTransition(movingState, attackingState, new FuncPredicate(() => inputReader.Attacking && !attackManager.IsAttacking));

        stateMachine.SetState(movingState);
    }

    private void OnEnable()
    {
        attackManager.OnAttackStarted += OnAttackStarted;
        attackManager.OnAttackFinished += OnAttackFinished;
    }

    private void OnAttackFinished()
    {
        SetVelocity(0, 0);
    }

    private void OnDisable()
    {
        attackManager.OnAttackStarted -= OnAttackStarted;
        attackManager.OnAttackFinished -= OnAttackFinished;
    }

    private void OnAttackStarted(AttackData data)
    {
        SetVelocity(data.Movement.x * Facing(), data.Movement.y);
    }

    public int Facing()
    {
        if (stateMachine.PreviousState == wallslidingState)
        {
            return -WallDirection();
        } else
        {
            return rotatePivot.transform.rotation == Quaternion.identity ? 1 : -1;
        }
    }

    private bool IsCollidingWithWall() => rightWallChecker.IsColliding || leftWallChecker.IsColliding;

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public void SetYVelocity(float y) => rigidBody.velocity = new Vector2(rigidBody.velocity.x, y);
    public void SetXVelocity(float x) => rigidBody.velocity = new Vector2(x, rigidBody.velocity.y);
    public void SetVelocity(float x, float y) => rigidBody.velocity = new Vector2(x, y);

    public void EnterAttackState()
    {
        attackManager.StartAttack();
    }

    public void EnterJumpingState()
    {
        coyoteTimer.Stop();

        if (stateMachine.PreviousState == wallslidingState)
        {
            float angleFromWall = 30;
            Vector2 rotatedVelocity = (Vector2.up * jumpForce).Rotated(angleFromWall * WallDirection());
            SetVelocity(rotatedVelocity.x, rotatedVelocity.y);

            SetFacing((int)Mathf.Sign(rotatedVelocity.x));
        } else
        {
            SetYVelocity(jumpForce);
        }
    }

    private int WallDirection() => rightWallChecker.IsColliding ? 1 : leftWallChecker.IsColliding ? -1 : 0;

    public void EnterWallSlideState()
    {
        SetFacing(WallDirection());
    }

    public void UpdateWallSlideState()
    {
        float speedMultiplier = inputReader.MoveDirection.y < 0 ? 1 : 0.8f;
        SetVelocity(0 , rigidBody.velocity.y * speedMultiplier);
    }

    public void EnterDashState()
    {
        int dashDirection = (int)inputReader.MoveDirection.x == 0 ? Facing() : (int)inputReader.MoveDirection.x;

        startingGravityScale = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;

        SetVelocity(dashDirection * dashSpeed, 0);
        SetFacing(dashDirection);
        dashDurationTimer.Start();
    }

    public void ExitDashState()
    {
        dashDurationTimer.Stop();
        rigidBody.gravityScale = startingGravityScale;
    }

    public void UpdateMovementState()
    {
        SetXVelocity(inputReader.MoveDirection.x * moveSpeed);

        if (inputReader.MoveDirection.x != 0)
            SetFacing((int)inputReader.MoveDirection.x);
    }

    public void UpdateJumpingAndFallingState()
    {
        float newXVelocity = Mathf.Clamp(rigidBody.velocity.x + airAccel * inputReader.MoveDirection.x * Time.deltaTime, -maxAirVelocity, maxAirVelocity);

        SetXVelocity(newXVelocity);

        if (newXVelocity != 0)
            SetFacing(Math.Sign(newXVelocity));
    }

    private void SetFacing(int dir)
    {
        Quaternion facingRightRotation = Quaternion.identity;
        Quaternion facingLeftRotation = Quaternion.Euler(180 * Vector3.up);

        rotatePivot.rotation = dir == 1 ? facingRightRotation : facingLeftRotation;
    }

    public void UpdateAnimatorVelocity()
    {
        animator.SetFloat("xVelocity", rigidBody.velocity.x);
        animator.SetFloat("yVelocity", rigidBody.velocity.y);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    
    public void StartAnimation(int animHash)
    {
        animator.CrossFade(animHash, 0);
    }

}
