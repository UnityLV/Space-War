using UnityEngine;

public class CursorRotationStrategy : IRotationStrategy
{
    private readonly IBoardObject _boardObject;
    private readonly IMouseInputSource _mouseInputSource;

    public CursorRotationStrategy(IBoardObject boardObject,IMouseInputSource mouseInputSource)
    {
        _boardObject = boardObject;
         _mouseInputSource = mouseInputSource;
    }

    public void Rotate()
    {
        Vector2 mousePos = new Vector2(_mouseInputSource.GetInput().X, _mouseInputSource.GetInput().Y);
        Vector2 direction = (mousePos - _boardObject.Position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _boardObject.Rotation = angle;
    }
}