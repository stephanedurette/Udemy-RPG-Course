using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : IState
{
    protected readonly int GroundedAnimHash = Animator.StringToHash("Grounded");
    protected readonly int JumpingAnimHash = Animator.StringToHash("Jumping");
    protected readonly int FallingAnimHash = Animator.StringToHash("Falling");
    protected readonly int DashingAnimHash = Animator.StringToHash("Dashing");
    protected readonly int WallslidingAnimHash = Animator.StringToHash("Wallsliding");
    protected readonly int KnockbackAnimHash = Animator.StringToHash("Knockback");
    protected readonly int blockingHash = Animator.StringToHash("Block");

    protected Player player;

    public PlayerBaseState(Player player)
    {
        this.player = player;
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        // noop
    }

    public virtual void Update()
    {
        player.UpdateAnimatorVelocity();
    }
}
