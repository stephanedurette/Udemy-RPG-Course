using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    public PlayerDashingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void OnEnter()
    {
        player.StartAnimation(DashingAnimHash);
        player.EnterDashState();
    }

    public override void OnExit()
    {
        player.ExitDashState();
    }

    public override void Update()
    {
        base.Update();
    }
}
