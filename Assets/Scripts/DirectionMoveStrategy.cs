using UnityEngine;

public class DirectionMoveStrategy : IMoveStrategy
{
    private readonly IBoardObject _boardObject;
    private readonly Vector2 _direction;

    private readonly int _speed = 20;
    public DirectionMoveStrategy(IBoardObject boardObject, Vector2 direction)
    {
        _boardObject = boardObject;
        _direction = direction;

    }

    public void Move()
    {
        _boardObject.Position += _direction * (_speed ) * IMoveStrategy.SpeedFactor;
    }
}