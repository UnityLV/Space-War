using UnityEngine;

public class BoardObjectSpawner
{
    private IBoard _board;

    public BoardObjectSpawner(IBoard board)
    {
        _board = board;
    }

    public IBoardObject SpawnBoardObject(Vector2 position = default, float rotation = default)
    {
        IBoardObject boardObject = new BoardObject();
        boardObject.Position = position;
        boardObject.Rotation = rotation;
        _board.AddObjectOnBoard(boardObject);
        boardObject.AddDependency(new Disposable(() => _board.RemoveObjectFromBoard(boardObject)));
        return boardObject;
    }
}