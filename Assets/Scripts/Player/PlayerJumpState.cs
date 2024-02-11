using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.Rigidbody.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.Rigidbody.velocity.y < 0)
        {
            playerStateMachine.ChangeState(player.fallState);
            return;
        }
    }
}
