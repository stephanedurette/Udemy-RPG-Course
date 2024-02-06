using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("walljumping");

        player.SetVelocity(5 * -player.facingDirection, player.jumpForce);
        stateTimer = 0.5f;
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

        if (stateTimer < 0)
        {
            playerStateMachine.ChangeState(player.fallState);
        }
    }
}
