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
        player.StartDash();
    }

    public override void OnExit()
    {
        player.StopDash();
    }

    public override void Update()
    {
        base.Update();
    }
}
