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

    [Header("Collision Check References")]
    [SerializeField] private BoxCaster groundChecker;
    [SerializeField] private BoxCaster rightWallChecker;
    [SerializeField] private BoxCaster leftWallChecker;

    [Header("Move Settings")]
    public float moveSpeed = 12f;
    public float jumpForce = 8f;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    private StateMachine stateMachine;

    //input states
    private Vector2 InputVector;
    private bool JumpInput;
    private bool DashInput;
    private bool AttackInput;

    //timers
    public CountdownTimer dashDurationTimer;
    public CountdownTimer coyoteTimer;
    CountdownTimer dashCooldownTimer;

    private float startingGravityScale;

    // Start is called before the first frame update
    void Awake()
    {
        //Timers
        dashDurationTimer = new CountdownTimer(dashDuration);
        dashCooldownTimer = new CountdownTimer(dashCooldown);
        coyoteTimer = new CountdownTimer(0.2f);

        dashDurationTimer.OnTimerStop += () => dashCooldownTimer.Start();

        //State Machine
        stateMachine = new StateMachine();

        var movingState = new PlayerMovementState(this);
        var jumpingState = new PlayerJumpingState(this);
        var fallingState = new PlayerFallingState(this);
        var wallslidingState = new PlayerWallslidingState(this);
        var dashingState = new PlayerDashingState(this);
        var attackingState = new PlayerAttackingState(this);

        //Jump
        stateMachine.AddTransition(movingState, jumpingState, new FuncPredicate(() => JumpInput && groundChecker.IsColliding));
        stateMachine.AddTransition(wallslidingState, jumpingState, new FuncPredicate(() => JumpInput ));
        stateMachine.AddTransition(fallingState, jumpingState, new FuncPredicate(() => JumpInput && coyoteTimer.IsRunning));

        //Fall
        stateMachine.AddTransition(jumpingState, fallingState, new FuncPredicate(() => rigidBody.velocity.y < 0));
        stateMachine.AddTransition(movingState, fallingState, new FuncPredicate(() => rigidBody.velocity.y < 0 && !groundChecker.IsColliding));
        stateMachine.AddTransition(wallslidingState, fallingState, new FuncPredicate(() => InputVector.x == -WallDirection()));
        stateMachine.AddTransition(dashingState, fallingState, new FuncPredicate(() => !dashDurationTimer.IsRunning));

        //Wallslide
        stateMachine.AddTransition(fallingState, wallslidingState, new FuncPredicate(WallSlidePredicate));

        //Moving
        stateMachine.AddTransition(fallingState, movingState, new FuncPredicate(() => groundChecker.IsColliding));
        stateMachine.AddTransition(wallslidingState, movingState, new FuncPredicate(() => groundChecker.IsColliding));
        stateMachine.AddTransition(attackingState, movingState, new FuncPredicate(() => !attackManager.IsAttacking));

        //Dashing
        stateMachine.AddAnyTransition(dashingState, new FuncPredicate(() => DashInput && !dashDurationTimer.IsRunning && !dashCooldownTimer.IsRunning));

        //Attacking
        stateMachine.AddTransition(movingState, attackingState, new FuncPredicate(() => AttackInput && !attackManager.IsAttacking));

        stateMachine.SetState(movingState);
    }

    bool WasWallSliding() => rigidBody.velocity.y < 0 && IsCollidingWithWall();

    private void Start()
    {

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
        SetVelocity(data.AttackMovement.x * PlayerSpriteFacing(), data.AttackMovement.y);
    }

    public int PlayerSpriteFacing()
    {
        if (WasWallSliding())
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

    public void StartAttack()
    {
        attackManager.StartAttack();
    }

    public void Jump()
    {
        if (WasWallSliding())
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

    private bool WallSlidePredicate()
    {
        bool collidingWithWall = IsCollidingWithWall();
        bool inputTowardsWall = (InputVector.x != -WallDirection());

        return collidingWithWall && inputTowardsWall;
    }

    private int WallDirection()
    {
        if (rightWallChecker.IsColliding) return 1;
        if (leftWallChecker.IsColliding) return -1;
        return 0;
    }

    public void HandleWallSlide()
    {
        float speedMultiplier = InputVector.y < 0 ? 1 : 0.8f;
        SetVelocity(0 , rigidBody.velocity.y * speedMultiplier);
    }

    public void StartDash()
    {
        int spriteFacing = PlayerSpriteFacing();

        startingGravityScale = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;

        SetVelocity(spriteFacing * dashSpeed, 0);
        SetFacing(spriteFacing);
        dashDurationTimer.Start();
    }

    public void StopDash()
    {
        dashDurationTimer.Stop();
        rigidBody.gravityScale = startingGravityScale;
    }

    public void HandleMovement()
    {
        SetXVelocity(InputVector.x * moveSpeed);

        if (InputVector.x != 0)
            SetFacing((int)InputVector.x);
    }

    public void HandleAirMovement()
    {
        if (InputVector.x != 0)
            SetXVelocity(InputVector.x * moveSpeed);

        if (InputVector.x != 0)
            SetFacing((int)InputVector.x);
    }

    private void SetFacing(int dir)
    {
        Quaternion facingRightRotation = Quaternion.identity;
        Quaternion facingLeftRotation = Quaternion.Euler(180 * Vector3.up);

        rotatePivot.rotation = dir == 1 ? facingRightRotation : facingLeftRotation;
    }

    public void UpdateInputs()
    {
        InputVector = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        AttackInput = Input.GetMouseButton(0);
        JumpInput = Input.GetKey(KeyCode.Space);
        DashInput = Input.GetKey(KeyCode.LeftShift);
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
