using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    public PlayerAttackingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void OnEnter()
    {
        player.StartAnimation(AttackingAnimHash);
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        base.Update();
    }
}
