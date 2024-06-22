using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBaseState : IState
{
    protected readonly int IdleAnimHash = Animator.StringToHash("Idle");
    protected readonly int WalkingAnimHash = Animator.StringToHash("Walk");

    protected Skeleton skeleton;

    public SkeletonBaseState(Skeleton s)
    {
        this.skeleton = s;
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        // noop
    }

    public virtual void Update()
    {
        
    }
}
