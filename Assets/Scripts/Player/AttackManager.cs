using ImprovedTimers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private AttackCycle attackCycle;
    [SerializeField] private Animator animator;

    public bool IsAttacking {  get; private set; }

    public Action<AttackData> OnAttackStarted = delegate { };
    public Action OnAttackFinished = delegate { };

    private int currentAttackIndex;

    private bool attackQueued;

    private CountdownTimer attackTimer;
    private CountdownTimer attackNullInputTimer;
    private CountdownTimer attackMoveDurationTimer;

    private void Awake()
    {
        attackTimer = new CountdownTimer(1);
        attackNullInputTimer = new CountdownTimer(0);
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

        AttackData currentAttackData = attackCycle.Attacks[currentAttackIndex].AttackData;

        GameObject attackObject = Instantiate(attackCycle.Attacks[currentAttackIndex].gameObject);
        attackObject.transform.SetParent(transform, false);
        attackObject.transform.localPosition = Vector3.zero;

        currentAttackIndex = (currentAttackIndex + 1) % attackCycle.Attacks.Count;

        animator.Play(currentAttackData.AnimationString);

        float animationLength = animator.runtimeAnimatorController.animationClips.First((clip) => clip.name == currentAttackData.AnimationString).length;

        attackTimer.Reset(animationLength);
        attackTimer.Start();

        attackNullInputTimer.Reset(animationLength / 2);
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

        if (attackTimer.IsRunning && inputReader.Attacking && !attackNullInputTimer.IsRunning)
            attackQueued = true;
    }
}
