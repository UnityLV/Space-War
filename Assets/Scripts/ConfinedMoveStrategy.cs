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