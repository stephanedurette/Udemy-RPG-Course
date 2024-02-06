using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.WallDetected() && xInput == player.facingDirection)
        {
            player.SetVelocity(0, player.Rigidbody.velocity.y);
        }
        else
        {
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, player.Rigidbody.velocity.y);
        }
    }
}
