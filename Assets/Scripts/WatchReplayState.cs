using System;

public class WatchReplayState : IState
{
    private Game _game;

    private Action _onReplayEnd;

    public WatchReplayState(Action onReplayEnd)
    {
        _onReplayEnd = onReplayEnd;
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
        Tools.ResetTime();
        Tools.InitSeed();
        
        _game = new Game();

        IBoard board = new Board();

        RecordedInputQueueSource runtimeInputQueueSource = new RecordedInputQueueSource();
        KeyboardInputConsumer keyboardInputConsumer = new KeyboardInputConsumer(runtimeInputQueueSource);

        _game.AddElement((keyboardInputConsumer));

        IBoardObjectFactory playerFactory =
            new BoardObjectFactoryBuilder()
                .SetBoard(board)
                .AddBrainCell(boardObject => new ShipBoardObjectView(boardObject))
                .AddBrainCell(boardObject => new InputMoveStrategy(boardObject, keyboardInputConsumer))
                .AddBrainCell(boardObject => new CursorRotationStrategy(boardObject))
                .AddBrainCell(boardObject => new InertiaMoveStrategy(boardObject, keyboardInputConsumer))
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
                    _onReplayEnd?.Invoke();
                }))
                .Build();

        IBoardObjectFactory projectileFactory = new BoardObjectFactoryBuilder()
            .SetBoard(board)
            .AddBrainCell(boardObject => new ProjectileBoardObjectView(boardObject))
            .AddBrainCell(boardObject =>
                boardObject.Position = player.Position + Tools.AngleToVector2(player.Rotation) * 1.5f)
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
        _game.AddElement(TickManager.StartTickable(new MouseClickTriggerTicker((Func<IBoardObject>)projectileFactory.Spawn)));

        _game.AddElement(projectileFactory);
        _game.AddElement(obstacleFactory);
        _game.AddElement(playerFactory);
    }

    public void Exit()
    {
        _game.Dispose();
    }
}