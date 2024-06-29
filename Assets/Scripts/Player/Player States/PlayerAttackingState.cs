using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackingState : PlayerBaseState
{
    public PlayerAttackingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void OnEnter()
    {
        player.EnterAttackState();
    }

    public override void OnExit()
    {
        
    }

    public override void Update()
    {
        base.Update();
        player.UpdateAttackState();
    }
}

public class PlayerAttack_1 : PlayerAttackingState
{
    public PlayerAttack_1(Player player) : base(player)
    {
    }

    public override void OnEnter()
    {
        player.CurrentAttackData = player.Attack1;
        base.OnEnter();
    }
}
public class PlayerAttack_2 : PlayerAttackingState
{
    public PlayerAttack_2(Player player) : base(player)
    {
    }

    public override void OnEnter()
    {
        player.CurrentAttackData = player.Attack2;
        base.OnEnter();
    }
}
public class PlayerAttack_3 : PlayerAttackingState
{
    public PlayerAttack_3(Player player) : base(player)
    {
    }

    public override void OnEnter()
    {
        player.CurrentAttackData = player.Attack3;
        base.OnEnter();
    }
}

