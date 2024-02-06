using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer <= 0)
        {
            playerStateMachine.ChangeState(player.dashState);
            return;
        }

        if (player.WallDetected() && xInput == player.facingDirection)
        {
            player.SetVelocity(0, player.Rigidbody.velocity.y);
        } else
        {
            player.SetVelocity(xInput * player.moveSpeed, player.Rigidbody.velocity.y);
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && player.IsOnGround())
        {
            playerStateMachine.ChangeState(player.jumpState);
        }

        if (!player.IsOnGround())
        {
            player.fallState.coyoteTimer = 0.2f;
            playerStateMachine.ChangeState(player.fallState);
        }
    }
}
