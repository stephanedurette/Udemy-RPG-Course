using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChaseState : SkeletonBaseState
{
    public SkeletonChaseState(Skeleton s) : base(s)
    {
    }

    public override void FixedUpdate()
    {
        // noop
    }

    public override void OnEnter()
    {
        skeleton.PlayAnimation(ChasingAnimHash);
    }

    public override void OnExit()
    {
        // noop
    }

    public override void Update()
    {
        skeleton.UpdateChaseState();
    }
}
