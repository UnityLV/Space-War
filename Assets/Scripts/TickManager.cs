using System;
using System.Collections.Generic;

public static class TickManager
{
    private static readonly List<ITickable> _tickables = new List<ITickable>();

    private static readonly List<ITickable> _iterableTickables = new List<ITickable>(); //iterable list to avoid collection modified exception

    public static IDisposable StartTickable(ITickable tickable)
    {
        _tickables.Add(tickable);

        return new Disposable(() => _tickables.Remove(tickable));
    }

    public static void Tick()
    {
        _iterableTickables.AddRange(_tickables);

        foreach (var tickable in _iterableTickables)
        {
            tickable.Tick();
        }

        _iterableTickables.Clear();
    }

    public static void Dispose()
    {
        _tickables.Clear();
    }
}