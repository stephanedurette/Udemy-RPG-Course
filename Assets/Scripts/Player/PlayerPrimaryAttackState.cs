using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private int numAttacksInCombo = 3;

    private bool mouseClickedDuringAnimation;

    public PlayerPrimaryAttackState(string animBoolName, PlayerStateMachine playerStateMachine, Player player) : base(animBoolName, playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Vector2 attackMovement = player.playerAttackMovement[comboCounter];

        int attackDirection = player.facingDirection;

        player.SetVelocity(attackMovement.x * attackDirection, attackMovement.y);
        stateTimer = 0.1f;

        player.Animator.SetInteger("ComboCounter", comboCounter);
        comboCounter = ((comboCounter + 1) % numAttacksInCombo);

        mouseClickedDuringAnimation = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetVelocity(0, player.Rigidbody.velocity.y);
        }

        bool mouseHeldDuringCombo = Input.GetMouseButton(0) && stateTimer < 0 && comboCounter != 0;
        bool mouseClickedDuringLastStrikeOfCombo = Input.GetMouseButtonDown(0) && comboCounter == 0;

        if (mouseHeldDuringCombo || mouseClickedDuringLastStrikeOfCombo)
        {
            mouseClickedDuringAnimation = true;
        }

        if (stateEndTrigger && mouseClickedDuringAnimation){
            playerStateMachine.ChangeState(player.playerPrimaryAttackState);
            return;
        }

        if (stateEndTrigger)
        {
            comboCounter = 0;
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
