using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallslideState : PlayerState
{
    public PlayerWallslideState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        player.Rigidbody.gravityScale = player.startingGravityScale;
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && player.facingDirection != xInput)
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if (player.IsOnGround())
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        player.Rigidbody.gravityScale = player.facingDirection == xInput ? 0f : player.startingGravityScale;

        float speedMultiplier = yInput < 0 ? 1 : 0.8f;

        player.SetVelocity(0, player.Rigidbody.velocity.y * speedMultiplier);
    }
}
