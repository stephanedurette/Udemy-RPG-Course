using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        player.HandleMovement();
    }

    public override void OnEnter()
    {
        player.StartAnimation(GroundedAnimHash);
    }

    public override void OnExit()
    {
        player.coyoteTimer.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}