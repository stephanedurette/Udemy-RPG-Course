using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerState
{
    private protected PlayerStateMachine playerStateMachine;
    private protected Player player;

    protected float xInput;

    private int animBoolNameHash;

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
    }
}
