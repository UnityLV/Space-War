using UnityEngine;

public class ConfinedMoveStrategy : IMoveStrategy
{
    private readonly IBoardObject _boardObject;
    private readonly Vector2 _min;
    private readonly Vector2 _max;

    public ConfinedMoveStrategy(IBoardObject boardObject, IBoard board)
    {
        _boardObject = boardObject;
        _min = Vector2.zero;
        _max = new Vector2(board.Size.x, board.Size.y);
    }

    public void Move()
    {
        _boardObject.Position = Vector2.Max(_min, Vector2.Min(_max, _boardObject.Position));
    }
}

public class InertiaMoveStrategy : IMoveStrategy
{
    private readonly IBoardObject _boardObject;
    private IKeyboardInputSource _keyboardInputSource;
    private Vector2 _inertia;
    private float _drag => 1f;
    private float _power => 20;

    public InertiaMoveStrategy(IBoardObject boardObject, IKeyboardInputSource keyboardInputSource)
    {
        _boardObject = boardObject;
        _keyboardInputSource = keyboardInputSource;
    }

    public void Move()
    {
        Vector2 input = new Vector2(_keyboardInputSource.GetInput().X, _keyboardInputSource.GetInput().Y);

        if (input.x != 0 && Mathf.Sign(input.x) != Mathf.Sign(_inertia.x)) _inertia.x = 0;
        if (input.y != 0 && Mathf.Sign(input.y) != Mathf.Sign(_inertia.y)) _inertia.y = 0;
    
        _inertia = Vector2.Lerp(_inertia, input, IMoveStrategy.SpeedFactor * _drag);
        _boardObject.Position += _inertia * _power * IMoveStrategy.SpeedFactor;
        _inertia = Vector2.Lerp(_inertia, input, IMoveStrategy.SpeedFactor * _drag);

    }
}