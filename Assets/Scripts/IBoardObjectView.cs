using System;

public interface IBoardObjectView : ITickable ,IDisposable
{
    void ITickable.Tick()
    {
        Draw();
    }

    void Draw();
}