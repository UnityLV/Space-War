using System.Collections.Generic;
using UnityEngine;

public class Board : IBoard
{
    public Vector2 Size { get; set; } = new Vector2(10, 10);
    
    private List<IBoardObject> _boardObjects = new List<IBoardObject>();
    
    public IReadOnlyList<IBoardObject> BoardObjects => _boardObjects;

    public void AddObjectOnBoard(IBoardObject boardObject)
    {
        _boardObjects.Add(boardObject);
    }
    
    public void RemoveObjectFromBoard(IBoardObject boardObject)
    {
        boardObject.Dispose();
        _boardObjects.Remove(boardObject);
    }
    
}

public interface IBoard
{
    Vector2 Size { get; set; }
    IReadOnlyList<IBoardObject> BoardObjects { get; }
    void AddObjectOnBoard(IBoardObject boardObject);
    void RemoveObjectFromBoard(IBoardObject boardObject);
}