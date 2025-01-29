using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardObject : IBoardObject
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public uint Id { get; set; } = IDGenerator.GenerateId();

    private readonly List<IDisposable> _disposables = new();
    private bool _disposed;

    public void AddDependency(IDisposable disposable)
    {
        _disposables.Add(disposable);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}