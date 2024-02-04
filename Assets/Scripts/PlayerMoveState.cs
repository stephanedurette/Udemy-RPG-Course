using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
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

        if (xInput == 0)
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        player.SetVelocity(xInput * player.moveSpeed, player.Rigidbody.velocity.y);
    }
}
