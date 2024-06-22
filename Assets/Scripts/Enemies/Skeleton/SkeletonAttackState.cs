using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : SkeletonBaseState
{
    public SkeletonAttackState(Skeleton s) : base(s)
    {
    }

    public override void FixedUpdate()
    {
        // noop
    }

    public override void OnEnter()
    {
        skeleton.EnterAttackState();
    }

    public override void OnExit()
    {
        // noop
    }

    public override void Update()
    {
        
    }
}
