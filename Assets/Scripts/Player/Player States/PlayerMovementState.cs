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
        player.UpdateMovementState();
    }

    public override void OnEnter()
    {
        player.StartAnimation(GroundedAnimHash);
    }

    public override void OnExit()
    {
        player.ExitMovementState();
    }

    public override void Update()
    {
        base.Update();
    }
}
