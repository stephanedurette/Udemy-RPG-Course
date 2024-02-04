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

        player.SetVelocity(xInput * player.moveSpeed, player.Rigidbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStateMachine.ChangeState(player.jumpState);
        }
    }
}
