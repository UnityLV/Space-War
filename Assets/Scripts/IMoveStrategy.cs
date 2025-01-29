using UnityEngine;

public interface IMoveStrategy : ITickable
{
    protected static float SpeedFactor => Time.deltaTime;
    void ITickable.Tick()
    {
        Move();
    }

    public void Move();
}