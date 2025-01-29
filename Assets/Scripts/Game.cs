using System;
using System.Collections.Generic;

public class Game
{
    private List<IDisposable> _someGameShit = new();

    public void AddElement(IDisposable disposable)
    {
        _someGameShit.Add(disposable);
    }
    
    public void Dispose()
    {
        foreach (var disposable in _someGameShit)
        {
            disposable.Dispose();
        }
    }
}