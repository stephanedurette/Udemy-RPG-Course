using ImprovedTimers;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skeleton : Entity
{

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
    [SerializeField] private float chaseCooldown = 2f;
    [SerializeField] private float playerExitCombatDistance = 7f;
    [SerializeField] private AttackData attackData;

    protected readonly int IdleAnimHash = Animator.StringToHash("Idle");
    protected readonly int WalkingAnimHash = Animator.StringToHash("Walk");
    protected readonly int ChasingAnimHash = Animator.StringToHash("Chase");
    protected readonly int HitAnimHash = Animator.StringToHash("Hit");

    private CountdownTimer idleTimer, attackCooldownTimer, chaseCooldownTimer, attackMoveTimer, attackDurationTimer;

    private Hitbox2D lastHitboxHit;
    private Vector2 lastPointHit;
    private Action WasHit;

    protected override void OnEnable()
    {
        base.OnEnable();
        animatorEvents.OnSpawnHitbox += SpawnHitbox;
    }

    private void SpawnHitbox(GameObject hitboxObject, float time)
    {
        hitboxObject.GetComponent<Hitbox2D>().OnHitboxHit += OnHitboxHit;
    }

    private void OnHitboxHit(Vector2 point, Hurtbox2D hurtBox)
    {
        Debug.Log("hit");
        Player p = hurtBox.Owner as Player;

        if (p.IsParrying(point))
        {
            Debug.Log("parry");
        }
    }

    protected override void SetupTimers()
    {
        idleTimer = new CountdownTimer(idleTime);
        attackCooldownTimer = new CountdownTimer(attackCooldown);
        chaseCooldownTimer = new CountdownTimer(chaseCooldown);

        attackDurationTimer = new CountdownTimer(0);
        attackMoveTimer = new CountdownTimer(0);
    }

    protected override void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        SkeletonIdleState idle = new SkeletonIdleState(this);
        SkeletonWalkingState walking = new SkeletonWalkingState(this);
        SkeletonChaseState chasing = new SkeletonChaseState(this);
        SkeletonAttackState attacking = new SkeletonAttackState(this);
        SkeletonKnockbackState knockBackState = new SkeletonKnockbackState(this);

        //to walking
        stateMachine.AddTransition(idle, walking, new FuncPredicate(() => !idleTimer.IsRunning));

        //to idle
        stateMachine.AddTransition(walking, idle, new FuncPredicate(() => !groundChecker.IsColliding || wallChecker.IsColliding));

        stateMachine.AddTransition(chasing, idle, new FuncPredicate(() => (!canSeePlayer.IsColliding && !chaseCooldownTimer.IsRunning)));
        stateMachine.AddTransition(chasing, idle, new FuncPredicate(() => !groundChecker.IsColliding));
        stateMachine.AddTransition(chasing, idle, new FuncPredicate(() => (Vector2.Distance(transform.position, Player.Instance.transform.position) >= playerExitCombatDistance) && !canSeePlayer.IsColliding));

        //to chase
        stateMachine.AddTransition(idle, chasing, new FuncPredicate(() => canSeePlayer.IsColliding && groundChecker.IsColliding));
        stateMachine.AddTransition(walking, chasing, new FuncPredicate(() => canSeePlayer.IsColliding));
        stateMachine.AddTransition(attacking, chasing, new FuncPredicate(() => !attackDurationTimer.IsRunning));

        //Knockback
        stateMachine.AddAnyTransition(knockBackState, new ActionInvokedPredicate(ref WasHit));
        stateMachine.AddTransition(knockBackState, chasing, new FuncPredicate(() => groundChecker.IsColliding && rigidBody.velocity.y <= .1f));

        //to attack
        stateMachine.AddTransition(chasing, attacking, new FuncPredicate(() => playerInRange.IsColliding && !attackCooldownTimer.IsRunning));


        stateMachine.SetState(idle);
    }



    public void EnterIdleState()
    {
        animator.Play(IdleAnimHash);
        SetXVelocity(0);
        idleTimer.Reset();
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
        chaseCooldownTimer.Reset();
        chaseCooldownTimer.Start();
    }

    protected override void OnHit(Vector2 point, Hitbox2D hitbox)
    {
        base.OnHit(point, hitbox);

        lastHitboxHit = hitbox;
        lastPointHit = point;

        WasHit?.Invoke();
    }

    public void EnterKnockbackState()
    {
        animator.Play(HitAnimHash);

        Vector2 knockbackDirection = new Vector2();
        knockbackDirection.x = Math.Sign(transform.position.x - lastPointHit.x);
        knockbackDirection.y = Mathf.Tan(lastHitboxHit.Data.knockbackAngleDegrees * Mathf.Deg2Rad);

        rigidBody.velocity = knockbackDirection.normalized * lastHitboxHit.Data.KnockbackForce;
    }

    public void UpdateChaseState()
    {
        int facing = GetFacing();
        float distance = Mathf.Abs(Player.Instance.transform.position.x - transform.position.x);
        int targetFacingDirection = (int)Mathf.Sign(Player.Instance.transform.position.x - transform.position.x);

        if (distance > .3f)
        {
            SetFacing(targetFacingDirection);
            SetXVelocity(playerInRange.IsColliding ? 0 : chaseSpeed * facing);
        }

        animator.Play(playerInRange.IsColliding ? IdleAnimHash : ChasingAnimHash);

        if (targetFacingDirection != facing) {
            canSeePlayer.Direction = -canSeePlayer.Direction;
        }
    }

    public void EnterAttackState()
    {
        attackCooldownTimer.Reset();
        attackCooldownTimer.Start();
        
        animator.Play(attackData.AnimationString);

        float animationLength = animator.runtimeAnimatorController.animationClips.First((clip) => clip.name == attackData.AnimationString).length;

        attackDurationTimer.Reset(animationLength);
        attackDurationTimer.Start();

        attackMoveTimer.Reset(attackData.MoveDuration);
        attackMoveTimer.Start();

        attackMoveTimer.OnTimerStop += () => SetVelocity(0, 0);

        SetVelocity(attackData.Movement.x * GetFacing(), attackData.Movement.y);

        SetXVelocity(0);
    }

    protected override void OnHeathHitZero()
    {
        Debug.Log("died");
    }

}
