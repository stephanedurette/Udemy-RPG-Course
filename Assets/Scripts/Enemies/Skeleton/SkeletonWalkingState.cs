using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWalkingState : SkeletonBaseState
{
    public SkeletonWalkingState(Skeleton s) : base(s)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void OnEnter()
    {
        skeleton.EnterWalkingState();
    }

    public override void OnExit()
    {
        skeleton.ExitWalkingState();
    }

    public override void Update()
    {
        
    }
}
