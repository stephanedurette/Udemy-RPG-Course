using ImprovedTimers;
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
    [SerializeField] private float idleTime = 1f;

    private StateMachine stateMachine;

    private CountdownTimer idleTimer;

    private void Awake()
    {
        SetupTimers();
        SetupStateMachine();
    }

    private void SetupTimers()
    {
        idleTimer = new CountdownTimer(idleTime);
    }

    private void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        SkeletonIdleState idle = new SkeletonIdleState(this);
        SkeletonWalkingState walking = new SkeletonWalkingState(this);

        //to walking
        stateMachine.AddTransition(idle, walking, new FuncPredicate(() => !idleTimer.IsRunning));

        //to idle
        stateMachine.AddTransition(walking, idle, new FuncPredicate(() => !groundChecker.IsColliding || wallChecker.IsColliding));

        stateMachine.SetState(idle);
    }

    public void PlayAnimation(int hash)
    {
        animator.Play(hash);
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
        SetXVelocity(0);
        idleTimer.Reset(idleTime);
        idleTimer.Start();
    }

    public void EnterWalkingState()
    {
        SetXVelocity(moveSpeed * GetFacing());
    }

    public void ExitWalkingState()
    {
        SetFacing(-GetFacing());
        canSeePlayer.Direction = -canSeePlayer.Direction;
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
