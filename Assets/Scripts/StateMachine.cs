using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : ITickable, IDisposable
{
    private IState _currentState;
    private readonly List<Transition> _transitions = new List<Transition>();

    public StateMachine(IState initialState)
    {
        ChangeState(initialState);
    }
    
    public void AddTransition(Transition transition)
    {
        _transitions.Add(transition);
        
        if (transition.FromState == _currentState)
        {
            transition.OnEnter();
        }
    }

    private void ChangeState(IState newState)
    {
        Debug. Log($"Changing state from {_currentState} to {newState}");
        foreach (var transition in _transitions)
        {
            if (transition.FromState == _currentState)
            {
                transition.OnExit();
            }
        }

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();

        foreach (var transition in _transitions)
        {
            if (transition.FromState == _currentState)
            {
                transition.OnEnter();
            }
        }
    }

    public void Tick()
    {
        foreach (var transition in _transitions)
        {
            if (transition.FromState == _currentState && transition.Condition())
            {
                ChangeState(transition.ToState);
                break;
            }
        }

        _currentState?.Update();
    }

    public void Dispose()
    {
        _currentState?.Exit();
    }
}