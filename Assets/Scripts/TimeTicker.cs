using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TimeTicker : ITickable
{
    private float _time;
    private float _tickTime;
    private Delegate _action;

    public TimeTicker(float tickTime, Delegate action)
    {
        _tickTime = tickTime;
        _action = action;
    }

    public void Tick()
    {
        _time += Time.deltaTime;
        if (_time >= _tickTime)
        {
            _time = 0;
            _action.DynamicInvoke();
        }
    }
}

public class TimeTrigger : ITickable
{
    private float _time;
    private float _tickTime;
    private Action _action;

    public TimeTrigger(float tickTime, Action action)
    {
        _tickTime = tickTime;

        _action = action;
    }

    public void Tick()
    {
        _time += Time.deltaTime;
        if (_time >= _tickTime)
        {
            _time = 0;
            _action.Invoke();
        }
    }
}

public class ConditionTriggerTicker : ITickable
{
    private Func<bool> _condition;
    private Action _action;

    public ConditionTriggerTicker(Func<bool> condition, Action action)
    {
        _condition = condition;
        _action = action;
    }

    public void Tick()
    {
        if (_condition.Invoke())
        {
            _action.Invoke();
        }
    }
}

public delegate bool ConditionWithItem(out IBoardObject item);

public class ConditionTriggerTickerWithItem : ITickable
{
    private ConditionWithItem _condition;
    private Action<IBoardObject> _action;

    public ConditionTriggerTickerWithItem(ConditionWithItem condition, Action<IBoardObject> action)
    {
        _condition = condition;
        _action = action;
    }

    public void Tick()
    {
        if (_condition.Invoke(out IBoardObject item))
        {
            _action.Invoke(item);
        }
    }
}

public class DistanceTrigger : ConditionTriggerTicker
{
    public DistanceTrigger(float distance, IBoardObject from, IBoardObject to, Action action) : base(
        () => CheepDistance(from, to) <= distance, action)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CheepDistance(IBoardObject from, IBoardObject to)
    {
        Vector2 distance = from.Position - to.Position;
        return distance.sqrMagnitude;
    }
}

public class MouseClickTriggerTicker : ITickable
{
    private readonly Delegate _action;
    private readonly IMouseInputSource _inputSource;
    private bool _slickLastTime;
    public MouseClickTriggerTicker( Delegate action,IMouseInputSource inputSource)
    {
        _action = action;
        _inputSource = inputSource;
    }

    public void Tick()
    {
        if (_inputSource.GetInput().LeftButton && !_slickLastTime)
        {
            _action.DynamicInvoke();
        }

        _slickLastTime = _inputSource.GetInput().LeftButton;

    }
}

public class CollectionDistanceTrigger : ConditionTriggerTickerWithItem
{
    public CollectionDistanceTrigger(float distance, IReadOnlyCollection<IBoardObject> from,
        IBoardObject to,
        Action<IBoardObject> action) : base((out IBoardObject item) => CheepDistance(from, to, out item) <= distance,
        action)
    {
    }

    private static float CheepDistance(IReadOnlyCollection<IBoardObject> from, IBoardObject to, out IBoardObject item)
    {
        float minDistance = float.MaxValue;
        item = null;
        foreach (IBoardObject boardObject in from)
        {
            float sqrMagnitude = DistanceTrigger.CheepDistance(boardObject, to);
            if (sqrMagnitude < minDistance)
            {
                minDistance = sqrMagnitude;
                item = boardObject;
            }
        }

        return minDistance;
    }
}