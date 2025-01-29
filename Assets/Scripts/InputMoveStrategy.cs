using System.Linq;
using UnityEngine;

public class InputMoveStrategy : IMoveStrategy 
{
    private readonly IBoardObject _boardObject;
    private int _speed = 10;
    private readonly IKeyboardInputSource _keyboardInputSource;

    public InputMoveStrategy(IBoardObject boardObject,IKeyboardInputSource keyboardInputSource)
    {
        _boardObject = boardObject;
        _keyboardInputSource =  keyboardInputSource;
    }

    public void Move()
    {
        Vector2 input = new Vector2(_keyboardInputSource.GetInput().X, _keyboardInputSource.GetInput().Y);
        input *= _speed * IMoveStrategy.SpeedFactor;
        _boardObject.Position += input;
    }
}

public class TargetMoveStrategy : IMoveStrategy
{
    private readonly IBoardObject _boardObject;
    private readonly IBoardObject _targetObject;
    private readonly Vector2 _direction;

    private int _speed = 5 + Random.Range(-1, 2);
    private float _missFactor = 1f;

    public TargetMoveStrategy(IBoardObject boardObject, IBoardObject targetObject)
    {
        _boardObject = boardObject;
        _targetObject = targetObject;
        _direction = GetDirectionThroughCenter();
    }

    private Vector2 GetDirectionThroughCenter()
    {
        Vector2 destination = _targetObject.Position;
        destination += Random.insideUnitCircle * _missFactor;
        Vector2 direction = (destination - _boardObject.Position).normalized;
        return direction;
    }

    public void Move()
    {
        _boardObject.Position += _direction * _speed * IMoveStrategy.SpeedFactor;
    }
}