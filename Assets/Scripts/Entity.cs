using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform attackParent;
    [SerializeField] protected Rigidbody2D rigidBody;
    [SerializeField] protected Transform rotatePivot;

    protected StateMachine stateMachine;

    protected Quaternion facingRightRotation = Quaternion.identity;
    protected Quaternion facingLeftRotation = Quaternion.Euler(180 * Vector3.up);

    protected virtual void Awake()
    {
        SetupTimers();
        SetupStateMachine();
    }

    protected virtual void SetupTimers()
    {

    }

    protected virtual void SetupStateMachine()
    {

    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public virtual void SetYVelocity(float y) => rigidBody.velocity = new Vector2(rigidBody.velocity.x, y);
    public virtual void SetXVelocity(float x) => rigidBody.velocity = new Vector2(x, rigidBody.velocity.y);
    public virtual void SetVelocity(float x, float y) => rigidBody.velocity = new Vector2(x, y);

    public virtual int GetFacing() => rotatePivot.transform.rotation == facingRightRotation ? 1 : -1;

    protected virtual void SetFacing(int dir)
    {
        rotatePivot.rotation = dir == 1 ? facingRightRotation : facingLeftRotation;
    }
}
