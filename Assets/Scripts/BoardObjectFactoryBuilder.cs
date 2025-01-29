using System;
using System.Collections.Generic;

public class BoardObjectFactoryBuilder : IBoardSetter, IConfigureActionSetter
{
    private IBoard _board;
    private readonly List<Action<IBoardObject>> _brainCells = new();
    private int BrainCellCount => _brainCells.Count; //Still more than you have

    public IConfigureActionSetter SetBoard(IBoard board)
    {
        _board = board;
        return this;
    }

    IConfigureActionSetter IConfigureActionSetter.AddBrainCell(Func<IBoardObject, object> brainCell)
    {
        _brainCells.Add(boardObject =>
        {
            object result = brainCell(boardObject);
            
            if (result is IDisposable disposable)
            {
                boardObject.AddDependency(disposable);
            }

            if (result is ITickable tickable)
            {
                boardObject.AddDependency(TickManager.StartTickable(tickable));
            }
        });
        return this;
    }

    public BoardObjectFactory Build()
    {
        return new BoardObjectFactory(_board, _brainCells);
    }
}

public interface IBoardSetter
{
    IConfigureActionSetter SetBoard(IBoard board);
}

public interface IConfigureActionSetter
{
    IConfigureActionSetter AddBrainCell(Func<IBoardObject, object> brainCell);
    BoardObjectFactory Build();
}