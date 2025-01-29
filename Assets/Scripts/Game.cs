using System;
using System.Collections.Generic;

public class Game
{
    private List<IDisposable> _someGameShit = new();

    public void AddElement(object element)
    {
        if (element is IDisposable disposable)
        {
            _someGameShit.Add(disposable);
        }
        if (element is ITickable tickable)
        {
            _someGameShit.Add(TickManager.StartTickable(tickable));
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