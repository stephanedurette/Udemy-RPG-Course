using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerState
{
    private string animBoolName;
    private PlayerStateMachine playerStateMachine;
    private Player player;

    public PlayerState(string animBoolName, PlayerStateMachine playerStateMachine, Player player)
    {
        this.animBoolName = animBoolName;
        this.playerStateMachine = playerStateMachine;
        this.player = player;
    }

    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public virtual void Update()
    {

    }
}
