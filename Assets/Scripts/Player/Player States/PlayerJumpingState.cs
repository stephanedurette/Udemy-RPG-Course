using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public PlayerJumpingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        player.HandleAirMovement();
    }

    public override void OnEnter()
    {
        player.StartAnimation(JumpingAnimHash);
        player.Jump();
        player.coyoteTimer.Stop();
    }

    public override void OnExit()
    {
        // noop
    }

    public override void Update()
    {
        base.Update();
    }
}
