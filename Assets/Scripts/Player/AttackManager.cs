using ImprovedTimers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AttackCycle attackCycle;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float attackNullInputDuration = .2f;

    public bool IsAttacking {  get; private set; }

    public Action<AttackData> OnAttackStarted = delegate { };
    public Action OnAttackFinished = delegate { };

    private int currentAttackIndex;

    private bool attackQueued;

    private bool attackInput;

    private CountdownTimer attackTimer;
    private CountdownTimer attackNullInputTimer;
    private CountdownTimer attackMoveDurationTimer;

    private void Awake()
    {
        attackTimer = new CountdownTimer(1);
        attackNullInputTimer = new CountdownTimer(attackNullInputDuration);
        attackMoveDurationTimer = new CountdownTimer(0);

        attackTimer.OnTimerStop += OnTimerStopped;
        attackMoveDurationTimer.OnTimerStop += () => OnAttackFinished?.Invoke();
    }

    public void StartAttack()
    {
        currentAttackIndex = 0;
        Attack();
    }

    private void Attack()
    {
        attackQueued = false;
        IsAttacking = true;

        AttackData currentAttackData = attackCycle.Attacks[currentAttackIndex];

        currentAttackIndex = (currentAttackIndex + 1) % attackCycle.Attacks.Count;

        animator.Play(currentAttackData.AttackAnimationString);

        attackTimer.Reset(currentAttackData.AttackDuration);
        attackTimer.Start();

        attackNullInputTimer.Reset();
        attackNullInputTimer.Start();

        attackMoveDurationTimer.Reset(currentAttackData.MoveDuration);
        attackMoveDurationTimer.Start();

        OnAttackStarted?.Invoke(currentAttackData);
    }

    void OnTimerStopped()
    {
        if (attackQueued)
        {
            Attack();
        } else
        {
            IsAttacking = false;
        }
    }

    public void Update()
    {
        attackInput = Input.GetMouseButton(0);

        if (attackTimer.IsRunning && attackInput && !attackNullInputTimer.IsRunning)
            attackQueued = true;
    }
}
