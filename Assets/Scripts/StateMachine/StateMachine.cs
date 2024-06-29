using System;
using System.Collections.Generic;
using UnityEngine.XR;

public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransitions = new();

    public Type CurrentState => current.State.GetType();

    public IState PreviousState { get; private set; }

    public void Update()
    {
        ITransition transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }

        current.State?.Update();
    }

    public void FixedUpdate()
    {
        current.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        current = nodes[state.GetType()];
        current.State?.OnEnter();
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }

    public void ChangeState(IState state)
    {
        if (state == current.State) return;

        PreviousState = current.State;
        var nextState = nodes[state.GetType()].State;

        PreviousState?.OnExit();
        nextState?.OnEnter();

        current = nodes[state.GetType()];

    }

    ITransition GetTransition()
    {
        foreach(var t in anyTransitions)
        {
            if (t.Condition.Evaluate())
            {
                return t;
            }
        }

        foreach (var t in current.Transitions)
        {
            if (t.Condition.Evaluate())
            {
                return t;
            }
        }

        return null;
    }
}
