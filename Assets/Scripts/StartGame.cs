﻿using System;

public class StartGame : IState
{
    private Game _game;

    private Action _onGameEnd;

    public StartGame(Action onGameEnd)
    {
        _onGameEnd = onGameEnd;
    }

    public void Enter()
    {
        Start();
    }

    public void Update()
    {
       
    }

    private void Start()
    {
        InputRecorder.ClearData();
        TimeInGame.ResetTime();
        Tools.InitSeed();
        
        _game = new Game();

        IBoard board = new Board();
        
        IInputQueue runtimeInputQueue = new RuntimeInputQueue();
        InputQueueConsumer inputQueueConsumer = new InputQueueConsumer(runtimeInputQueue);

        _game.AddElement(runtimeInputQueue);
        _game.AddElement(inputQueueConsumer);
        _game.AddElement(new InputRecorder(runtimeInputQueue));

        IBoardObjectFactory playerFactory =
            new BoardObjectFactoryBuilder()
                .SetBoard(board)
                .AddBrainCell(boardObject => new ShipBoardObjectView(boardObject))
                .AddBrainCell(boardObject => new InputMoveStrategy(boardObject, inputQueueConsumer))
                .AddBrainCell(boardObject => new CursorRotationStrategy(boardObject,inputQueueConsumer))
                .AddBrainCell(boardObject => new InertiaMoveStrategy(boardObject, inputQueueConsumer))
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
                .AddBrainCell(boardObject => new DistanceTrigger(1, boardObject, player,()=>
                {
                    boardObject.Dispose();
                    _onGameEnd?.Invoke();
                }))
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

        _game.AddElement(TickManager.StartTickable(new TimeTicker(.5f, (Func<IBoardObject>)obstacleFactory.Spawn)));
        _game.AddElement(TickManager.StartTickable(new MouseClickTriggerTicker((Func<IBoardObject>)projectileFactory.Spawn,inputQueueConsumer)));

        _game.AddElement(projectileFactory);
        _game.AddElement(obstacleFactory);
        _game.AddElement(playerFactory);
    }

    public void Exit()
    {
        _game.Dispose();
    }
}