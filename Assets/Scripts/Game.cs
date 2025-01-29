using System;
using System.Collections.Generic;

public class Game
{
    private List<IDisposable> _someGameShit = new();

    public void AddElement(IDisposable disposable)
    {
        if (disposable is ITickable tickable)
        {
            _someGameShit.Add(TickManager.StartTickable(tickable));
        }

        _someGameShit.Add(disposable);
    }

    public void AddElement(ITickable tickable)
    {
        _someGameShit.Add(TickManager.StartTickable(tickable));

        if (tickable is IDisposable disposable)
        {
            _someGameShit.Add(disposable);
        }
    }

    public void Dispose()
    {
        foreach (var disposable in _someGameShit)
        {
            disposable.Dispose();
        }
    }
}