using System;
using System.Collections.Generic;

public interface IBoardObjectFactory : IDisposable
{
    public IReadOnlyList<IBoardObject> Objects { get; }
    IBoardObject Spawn();
    
    void IDisposable.Dispose()
    {
        IList<IBoardObject> objects = new List<IBoardObject>(Objects);
        foreach (var obj in objects)
        {
            obj.Dispose();
        }
    }
}

