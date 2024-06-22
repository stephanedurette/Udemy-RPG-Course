using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallslidingState : PlayerBaseState
{
    public PlayerWallslidingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        player.HandleWallSlide();
    }

    public override void OnEnter()
    {
        player.StartAnimation(WallslidingAnimHash);
        player.EnterWallSlide();
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
