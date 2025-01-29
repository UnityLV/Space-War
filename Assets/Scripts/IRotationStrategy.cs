public interface IRotationStrategy : ITickable
{
    void ITickable.Tick()
    {
        Rotate();
    }
    void Rotate();
}