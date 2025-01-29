using System;
using UnityEngine;

public interface IBoardObject : IDisposable
{
    Vector2 Position { get; set; }
    float Rotation { get; set; }
    uint Id { get;}
    void AddDependency(IDisposable disposable);
}