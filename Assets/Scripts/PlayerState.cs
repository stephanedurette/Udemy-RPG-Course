using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerState
{
    private protected PlayerStateMachine playerStateMachine;
    private protected Player player;

    protected float xInput;

    private protected int animBoolNameHash;

    private protected float stateTimer;

    private static protected float dashUsageTimer;

    public PlayerState(string animBoolName, PlayerStateMachine playerStateMachine, Player player)
    {
        this.animBoolNameHash = Animator.StringToHash(animBoolName);
        this.playerStateMachine = playerStateMachine;
        this.player = player;
    }

    public virtual void Enter()
    {
        player.Animator.SetBool(animBoolNameHash, true);
    }
    public virtual void Exit()
    {
        player.Animator.SetBool(animBoolNameHash, false);
    }
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        player.Animator.SetFloat("xVelocity", player.Rigidbody.velocity.x);
        player.Animator.SetFloat("yVelocity", player.Rigidbody.velocity.y);

        stateTimer -= Time.deltaTime;
        dashUsageTimer -= Time.deltaTime;

        CheckDashState();
    }

    private void CheckDashState()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer <= 0)
        {
            playerStateMachine.ChangeState(player.dashState);
        }
    }
}
