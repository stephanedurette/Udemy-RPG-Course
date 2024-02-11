using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private int dashDirection;

    public PlayerDashState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;

        dashUsageTimer = player.dashCooldown + player.dashDuration;

        float playerXInput = Input.GetAxisRaw("Horizontal");
        dashDirection = playerXInput != 0 ? (int)playerXInput : player.facingDirection;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(dashDirection * player.dashSpeed, 0);

        if (player.WallDetected())
        {
            playerStateMachine.ChangeState(player.IsOnGround() ? player.idleState : player.wallslideState);
            return;
        }

        if (stateTimer <= 0)
        {
            playerStateMachine.ChangeState(player.IsOnGround() ? player.idleState : player.fallState);
            return;
        }
    }
}
