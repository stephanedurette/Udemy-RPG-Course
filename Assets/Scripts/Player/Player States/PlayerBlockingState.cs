using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void OnEnter()
    {
        player.StartAnimation(blockingHash);
        player.EnterBlockingState();
    }

    public override void OnExit()
    {

        player.ExitBlockingState();
    }

    public override void Update()
    {
        
    }
}



