using ImprovedTimers;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform rotatePivot;

    [Header("Wall Collision Check References")]
    [SerializeField] private BoxCaster groundChecker;
    [SerializeField] private BoxCaster wallChecker;

    [Header("Player Detection Check References")]
    [SerializeField] private RayCaster canSeePlayer;
    [SerializeField] private BoxCaster playerInRange;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float chaseSpeed = 12f;
    [SerializeField] private float idleTime = 1f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = .4f;

    protected readonly int IdleAnimHash = Animator.StringToHash("Idle");
    protected readonly int WalkingAnimHash = Animator.StringToHash("Walk");
    protected readonly int ChasingAnimHash = Animator.StringToHash("Chase");
    protected readonly int AttackingAnimHash = Animator.StringToHash("Attack");

    private StateMachine stateMachine;

    private CountdownTimer idleTimer, attackCooldownTimer;

    private void Awake()
    {
        SetupTimers();
        SetupStateMachine();
    }

    private void SetupTimers()
    {
        idleTimer = new CountdownTimer(idleTime);
        attackCooldownTimer = new CountdownTimer(attackCooldown);
    }

    private void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        SkeletonIdleState idle = new SkeletonIdleState(this);
        SkeletonWalkingState walking = new SkeletonWalkingState(this);
        SkeletonChaseState chasing = new SkeletonChaseState(this);
        SkeletonAttackState attacking = new SkeletonAttackState(this);

        //to walking
        stateMachine.AddTransition(idle, walking, new FuncPredicate(() => !idleTimer.IsRunning));

        //to idle
        stateMachine.AddTransition(walking, idle, new FuncPredicate(() => !groundChecker.IsColliding || wallChecker.IsColliding));

        //to chase
        stateMachine.AddTransition(idle, chasing, new FuncPredicate(() => canSeePlayer.IsColliding));
        stateMachine.AddTransition(walking, chasing, new FuncPredicate(() => canSeePlayer.IsColliding));
        stateMachine.AddTransition(attacking, chasing, new FuncPredicate(() => !animator.IsPlaying("Attack")));

        //to attack
        stateMachine.AddTransition(chasing, attacking, new FuncPredicate(() => playerInRange.IsColliding && !attackCooldownTimer.IsRunning));


        stateMachine.SetState(idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void EnterIdleState()
    {
        animator.Play(IdleAnimHash);
        SetXVelocity(0);
        idleTimer.Reset(idleTime);
        idleTimer.Start();
    }

    public void EnterWalkingState()
    {
        animator.Play(WalkingAnimHash);
        SetXVelocity(moveSpeed * GetFacing());
    }

    public void ExitWalkingState()
    {
        SetFacing(-GetFacing());
        canSeePlayer.Direction = -canSeePlayer.Direction;
    }

    public void EnterChaseState()
    {
        
    }

    public void UpdateChaseState()
    {
        int facing = GetFacing();
        int targetFacingDirection = (int)Mathf.Sign(Player.Instance.transform.position.x - transform.position.x);

        SetFacing(targetFacingDirection);
        SetXVelocity(playerInRange.IsColliding ? 0 : chaseSpeed * GetFacing());
        animator.Play(playerInRange.IsColliding ? IdleAnimHash : ChasingAnimHash);

        if (targetFacingDirection != facing) {
            canSeePlayer.Direction = -canSeePlayer.Direction;
        }
    }

    public void EnterAttackState()
    {
        animator.Play(AttackingAnimHash);
        attackCooldownTimer.Reset(attackCooldown);
        attackCooldownTimer.Start();
        SetXVelocity(0);
    }

    private void SetFacing(int dir)
    {
        Quaternion facingRightRotation = Quaternion.identity;
        Quaternion facingLeftRotation = Quaternion.Euler(180 * Vector3.up);

        rotatePivot.rotation = dir == 1 ? facingRightRotation : facingLeftRotation;
    }

    public int GetFacing()
    {
        return rotatePivot.transform.rotation == Quaternion.identity ? 1 : -1; 
    }

    public void SetYVelocity(float y) => rigidBody.velocity = new Vector2(rigidBody.velocity.x, y);
    public void SetXVelocity(float x) => rigidBody.velocity = new Vector2(x, rigidBody.velocity.y);
    public void SetVelocity(float x, float y) => rigidBody.velocity = new Vector2(x, y);

}
