using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStateMachine stateMachine;
    private PlayerMoveState moveState;
    private PlayerIdleState idleState;

    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = new PlayerStateMachine();

        moveState = new PlayerMoveState("Move", stateMachine, this);
        idleState = new PlayerIdleState("Idle", stateMachine, this);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.CurrentState.Update();
    }
}
