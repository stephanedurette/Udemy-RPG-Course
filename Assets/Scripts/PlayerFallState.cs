using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public float coyoteTimer;

    public PlayerFallState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        coyoteTimer = 0;
    }

    public override void Update()
    {
        base.Update();
        coyoteTimer -= Time.deltaTime;

        if (player.WallDetected() && player.Rigidbody.velocity.y < 0)
        {
            playerStateMachine.ChangeState(player.wallslideState);
        }

        if (player.IsOnGround())
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0)
        {
            playerStateMachine.ChangeState(player.jumpState);
        }
    }
}
