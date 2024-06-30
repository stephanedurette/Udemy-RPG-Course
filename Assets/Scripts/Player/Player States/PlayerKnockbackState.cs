using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerBaseState
{
    public PlayerKnockbackState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void OnEnter()
    {
        player.StartAnimation(KnockbackAnimHash);
        player.EnterKnockbackState();
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        base.Update();
    }
}
