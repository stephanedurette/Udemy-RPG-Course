using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonBaseState
{
    public SkeletonIdleState(Skeleton s) : base(s)
    {
    }

    public override void FixedUpdate()
    {
        // noop
    }

    public override void OnEnter()
    {
        skeleton.EnterIdleState();
    }

    public override void OnExit()
    {
        // noop
    }

    public override void Update()
    {
        
    }
}
