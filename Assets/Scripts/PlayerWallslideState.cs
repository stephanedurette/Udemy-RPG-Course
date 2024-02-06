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
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) {
            playerStateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDirection != xInput)
        {
            playerStateMachine.ChangeState(player.fallState);
        }

        if (player.IsOnGround())
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        float speedMultiplier = yInput < 0 ? 1 : 0.8f;

        player.SetVelocity(0, player.Rigidbody.velocity.y * speedMultiplier);
    }
}
