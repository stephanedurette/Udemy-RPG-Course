using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnockbackState : SkeletonBaseState
{
    public SkeletonKnockbackState(Skeleton s) : base(s)
    {
    }

    public override void FixedUpdate()
    {
        // noop
    }

    public override void OnEnter()
    {
        skeleton.EnterKnockbackState();
    }

    public override void OnExit()
    {
        // noop
    }

    public override void Update()
    {
        
    }
}
