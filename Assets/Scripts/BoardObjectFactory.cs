using System;
using System.Collections.Generic;

public class BoardObjectFactory : IBoardObjectFactory
{
    private readonly List<Action<IBoardObject>> _configurators;

    private readonly IBoard _board;

    private readonly List<IBoardObject> _existing = new();
    public IReadOnlyList<IBoardObject> Objects => _existing;

    public BoardObjectFactory(IBoard board, List<Action<IBoardObject>> configurators)
    {
        _board = board;
        _configurators = configurators;
    }

    public IBoardObject Spawn()
    {
        IBoardObject boardObject = new BoardObjectSpawner(_board).SpawnBoardObject();
        _existing.Add(boardObject);
        
        foreach (var action in _configurators)
        {
            action(boardObject);
        }
        
        boardObject.AddDependency(new Disposable(() => _existing.Remove(boardObject)));
        return boardObject;
    }
}

