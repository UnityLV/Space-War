using UnityEngine;

public class CursorRotationStrategy : IRotationStrategy
{
    private readonly IBoardObject _boardObject;

    public CursorRotationStrategy(IBoardObject boardObject)
    {
        _boardObject = boardObject;
    }

    public void Rotate()
    {
        Vector2 mousePos = Vector2.zero;
        Vector2 direction = (mousePos - _boardObject.Position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _boardObject.Rotation = angle;
    }
}