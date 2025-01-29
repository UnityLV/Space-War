// Purpose: Entry point of the game. Initializes the game and starts it.
using System;
using UnityEngine;
using static TickManager;

public class Root : MonoBehaviour
{
    private Game _game;
    
    private void OnEnable()
    {
        StartGame();
    }

    private void StartGame()
    {
        _game = new Game();
        
        IBoard board = new Board();

        IBoardObjectFactory playerFactory =
            new BoardObjectFactoryBuilder()
                .SetBoard(board)
                .AddBrainCell(boardObject => new ShipBoardObjectView(boardObject))
                .AddBrainCell(boardObject => new InputMoveStrategy(boardObject))
                .AddBrainCell(boardObject => new CursorRotationStrategy(boardObject))
                .AddBrainCell(boardObject => new ConfinedMoveStrategy(boardObject, board))
                .Build();

        IBoardObject player = playerFactory.Spawn();

        IBoardObjectFactory obstacleFactory =
            new BoardObjectFactoryBuilder()
                .SetBoard(board)
                .AddBrainCell(boardObject => new ObstacleBoardObjectView(boardObject))
                .AddBrainCell(boardObject => boardObject.Position = Tools.GetPositionOutsideScreen())
                .AddBrainCell(boardObject => new TargetMoveStrategy(boardObject, player))
                .AddBrainCell(boardObject => new TimeTrigger(10, boardObject.Dispose))
                .AddBrainCell(boardObject => new DistanceTrigger(1, boardObject, player, boardObject.Dispose))
                .Build();

        IBoardObjectFactory projectileFactory = new BoardObjectFactoryBuilder()
            .SetBoard(board)
            .AddBrainCell(boardObject => new ProjectileBoardObjectView(boardObject))
            .AddBrainCell(boardObject => boardObject.Position = player.Position + Tools.AngleToVector2(player.Rotation) * 1.5f)
            .AddBrainCell(boardObject => boardObject.Rotation = player.Rotation)
            .AddBrainCell(boardObject => new TimeTrigger(1, boardObject.Dispose))
            .AddBrainCell(boardObject => new DirectionMoveStrategy(boardObject, Tools.AngleToVector2(player.Rotation)))
            .AddBrainCell(boardObject => new CollectionDistanceTrigger(1, obstacleFactory.Objects, boardObject,
                obstacle =>
                {
                    obstacle.Dispose();
                    boardObject.Dispose();
                }))
            .Build();

        _game.AddElement(StartTickable(new TimeTicker(.5f, (Func<IBoardObject>)obstacleFactory.Spawn)));
        _game.AddElement(StartTickable(new MouseClickTriggerTicker((Func<IBoardObject>)projectileFactory.Spawn)));

        _game.AddElement(projectileFactory);
        _game.AddElement(obstacleFactory);
        _game.AddElement(playerFactory);
    }

    private void Update()
    {
        Tick();
    }

    private void OnDisable()
    {
        _game.Dispose();
        Dispose();
    }
}