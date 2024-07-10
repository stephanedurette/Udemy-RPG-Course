using ImprovedTimers;
using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : Entity
{
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

    [Header("Attack Settings")]
    [SerializeField] private float attackQueueNullInputDuration = .1f;
    public AttackData Attack1;
    public AttackData Attack2;
    public AttackData Attack3;

    [Header("Block Settings")]
    public float parryTime = .2f;
    public float blockKnockbackForce = .5f;
    public PhysicsMaterial2D blockMaterial;
    public PhysicsMaterial2D defaultMaterial;

    [SerializeField] private float knockbackDuration = .2f;

    //timers
    private CountdownTimer dashDurationTimer;
    private CountdownTimer coyoteTimer;
    private CountdownTimer dashCooldownTimer;

    private CountdownTimer attackDurationTimer;
    private CountdownTimer attackMoveDurationTimer;
    private CountdownTimer attackNullInputTimer;
    private CountdownTimer knockbackDurationTimer;

    private CountdownTimer parryTimer;

    private float startingGravityScale;
    private bool nextAttackQueued;

    [HideInInspector] public AttackData CurrentAttackData;

    [HideInInspector] public bool IsParrying => parryTimer.IsRunning;

    IState movingState, jumpingState, fallingState, wallslidingState, dashingState, knockbackState;
    IState attack1State, attack2State, attack3State, blockingState;

    private Hitbox2D lastHitboxHit;
    private Vector2 lastPointHit;

    private Action WasHit;

    // Start is called before the first frame update
    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    protected override void SetupTimers()
    {
        dashDurationTimer = new CountdownTimer(dashDuration);
        dashCooldownTimer = new CountdownTimer(dashCooldown);
        coyoteTimer = new CountdownTimer(0.2f);

        knockbackDurationTimer = new CountdownTimer(knockbackDuration);

        attackDurationTimer = new CountdownTimer(0);
        attackMoveDurationTimer = new CountdownTimer(0);
        attackNullInputTimer = new CountdownTimer(attackQueueNullInputDuration);

        parryTimer = new CountdownTimer(parryTime);
        
        dashDurationTimer.OnTimerStop += () => dashCooldownTimer.Start();
    }

    protected override void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        movingState = new PlayerMovementState(this);
        jumpingState = new PlayerJumpingState(this);
        fallingState = new PlayerFallingState(this);
        wallslidingState = new PlayerWallslidingState(this);
        dashingState = new PlayerDashingState(this);
        knockbackState = new PlayerKnockbackState(this);
        blockingState = new PlayerBlockingState(this);

        attack1State = new PlayerAttack_1(this);
        attack2State = new PlayerAttack_2(this);
        attack3State = new PlayerAttack_3(this);

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

        //Dashing
        stateMachine.AddTransition(movingState, dashingState, new FuncPredicate(() => inputReader.Dashing && !dashDurationTimer.IsRunning && !dashCooldownTimer.IsRunning));
        stateMachine.AddTransition(jumpingState, dashingState, new FuncPredicate(() => inputReader.Dashing && !dashDurationTimer.IsRunning && !dashCooldownTimer.IsRunning));
        stateMachine.AddTransition(fallingState, dashingState, new FuncPredicate(() => inputReader.Dashing && !dashDurationTimer.IsRunning && !dashCooldownTimer.IsRunning));
        stateMachine.AddTransition(wallslidingState, dashingState, new FuncPredicate(() => inputReader.Dashing && !dashDurationTimer.IsRunning && !dashCooldownTimer.IsRunning));

        //Knockback
        stateMachine.AddAnyTransition(knockbackState, new ActionInvokedPredicate(ref WasHit));
        stateMachine.AddTransition(knockbackState, fallingState, new FuncPredicate(() => !knockbackDurationTimer.IsRunning));

        //Attacking
        stateMachine.AddTransition(movingState, attack1State, new FuncPredicate(() => inputReader.Attacking));
        stateMachine.AddTransition(attack1State, attack2State, new FuncPredicate(() => !attackDurationTimer.IsRunning && nextAttackQueued));
        stateMachine.AddTransition(attack2State, attack3State, new FuncPredicate(() => !attackDurationTimer.IsRunning && nextAttackQueued));

        stateMachine.AddTransition(attack1State, movingState, new FuncPredicate(() => !attackDurationTimer.IsRunning));
        stateMachine.AddTransition(attack2State, movingState, new FuncPredicate(() => !attackDurationTimer.IsRunning));
        stateMachine.AddTransition(attack3State, movingState, new FuncPredicate(() => !attackDurationTimer.IsRunning));

        //Blocking
        stateMachine.AddTransition(movingState, blockingState, new FuncPredicate(() => inputReader.Blocking));
        stateMachine.AddTransition(blockingState, movingState, new FuncPredicate(() => !inputReader.Blocking));
        //
        stateMachine.SetState(movingState);
    }

    public override int GetFacing()
    {
        if (stateMachine.PreviousState == wallslidingState)
        {
            return -WallDirection();
        }
        else
        {
            return rotatePivot.transform.rotation == facingRightRotation ? 1 : -1;
        }
    }

    private bool IsCollidingWithWall() => rightWallChecker.IsColliding || leftWallChecker.IsColliding;

    public void EnterAttackState()
    {
        nextAttackQueued = false;

        animator.Play(CurrentAttackData.AnimationString);

        float animationLength = animator.runtimeAnimatorController.animationClips.First((clip) => clip.name == CurrentAttackData.AnimationString).length;

        attackDurationTimer.Reset(animationLength);
        attackDurationTimer.Start();

        attackNullInputTimer.Reset();
        attackNullInputTimer.Start();

        attackMoveDurationTimer.Reset(CurrentAttackData.MoveDuration);
        attackMoveDurationTimer.Start();

        attackMoveDurationTimer.OnTimerStop += () => SetVelocity(0, 0);

        SetVelocity(CurrentAttackData.Movement.x * GetFacing(), CurrentAttackData.Movement.y);
    }

    protected override void OnHit(Vector2 point, Hitbox2D hitbox)
    {
        bool isAttackSourceInBlockDir = Math.Sign(point.x - transform.position.x) == GetFacing();

        //blocking
        if (stateMachine.CurrentIState == blockingState && isAttackSourceInBlockDir)
        {
            if (IsParrying)
            {

            } 
            else
            {
                SetXVelocity(-GetFacing() * blockKnockbackForce);
            }

            return;
        }

        lastHitboxHit = hitbox;
        lastPointHit = point;

        base.OnHit(point, hitbox);
        WasHit?.Invoke();
    }

    public void UpdateAttackState()
    {
        if (inputReader.Attacking && !attackNullInputTimer.IsRunning)
        {
            nextAttackQueued = true;
        }
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
        }
        else
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
        SetVelocity(0, rigidBody.velocity.y * speedMultiplier);
    }

    public void EnterDashState()
    {
        int dashDirection = (int)inputReader.MoveDirection.x == 0 ? GetFacing() : (int)inputReader.MoveDirection.x;

        startingGravityScale = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;

        SetVelocity(dashDirection * dashSpeed, 0);
        SetFacing(dashDirection);
        dashDurationTimer.Start();
    }

    public void ExitWallslideState()
    {
        coyoteTimer.Start();
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

    public void EnterKnockbackState()
    {
        knockbackDurationTimer.Reset();
        knockbackDurationTimer.Start();

        Vector2 knockbackDirection = new Vector2();
        knockbackDirection.x = Math.Sign(transform.position.x - lastPointHit.x);
        knockbackDirection.y = Mathf.Tan(lastHitboxHit.Data.knockbackAngleDegrees * Mathf.Deg2Rad);

        rigidBody.velocity = knockbackDirection.normalized * lastHitboxHit.Data.KnockbackForce;
    }

    public void ExitMovementState()
    {
        coyoteTimer.Start();
    }

    public void UpdateJumpingAndFallingState()
    {
        float newXVelocity = Mathf.Clamp(rigidBody.velocity.x + airAccel * inputReader.MoveDirection.x * Time.deltaTime, -maxAirVelocity, maxAirVelocity);

        SetXVelocity(newXVelocity);

        if (newXVelocity != 0)
            SetFacing(Math.Sign(newXVelocity));
    }

    public void UpdateAnimatorVelocity()
    {
        animator.SetFloat("xVelocity", rigidBody.velocity.x);
        animator.SetFloat("yVelocity", rigidBody.velocity.y);
    }


    public void StartAnimation(int animHash)
    {
        animator.CrossFade(animHash, 0);
    }

    public void EnterBlockingState()
    {
        SetXVelocity(0f);
        rigidBody.sharedMaterial = blockMaterial;
        parryTimer.Start();
    }

    public void ExitBlockingState()
    {
        rigidBody.sharedMaterial = defaultMaterial;
        parryTimer.Stop();
    }

}
