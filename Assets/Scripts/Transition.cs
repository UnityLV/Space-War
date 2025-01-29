using System;

public abstract class Transition
{
    public IState FromState { get; }
    public IState ToState { get; }

    protected Transition(IState fromState, IState toState)
    {
        FromState = fromState;
        ToState = toState;
    }

    public virtual void OnEnter()
    {
        // Code to execute when entering the state
    }

    public virtual void OnExit()
    {
        // Code to execute when exiting the state
    }

    public abstract bool Condition();
}