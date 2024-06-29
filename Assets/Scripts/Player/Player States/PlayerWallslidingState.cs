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
        player.UpdateWallSlideState();
    }

    public override void OnEnter()
    {
        player.StartAnimation(WallslidingAnimHash);
        player.EnterWallSlideState();
    }

    public override void OnExit()
    {
        player.ExitWallslideState();
    }

    public override void Update()
    {
        base.Update();
    }
}
