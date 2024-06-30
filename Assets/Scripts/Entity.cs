using System;
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
    [SerializeField] protected Hurtbox2D hurtBox;

    [Header("Heath Settings")]
    [SerializeField] private int maxHealth;

    protected StateMachine stateMachine;

    protected Health health;

    protected Quaternion facingRightRotation = Quaternion.identity;
    protected Quaternion facingLeftRotation = Quaternion.Euler(180 * Vector3.up);

    protected virtual void Awake()
    {
        SetupTimers();
        SetupStateMachine();

        health = new Health(maxHealth);
    }

    protected virtual void OnEnable()
    {
        health.OnHealthHitZero += OnHeathHitZero;
        hurtBox.OnHurtBoxHit += OnHit;
    }

    protected virtual void OnHit(Vector2 point, Hitbox2D hitbox)
    {
        health.TakeDamage(hitbox.Data.Damage);
    }

    protected virtual void OnHeathHitZero()
    {
    }

    protected virtual void OnDisable()
    {
        health.OnHealthHitZero -= OnHeathHitZero;
        hurtBox.OnHurtBoxHit -= OnHit;
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
