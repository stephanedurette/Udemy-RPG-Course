using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        player.HandleAirMovement();
    }

    public override void OnEnter()
    {
        player.StartAnimation(FallingAnimHash);
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
