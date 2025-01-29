// Purpose: Entry point of the game. Initializes the game and starts it.
using UnityEngine;

public class Root : MonoBehaviour
{
    private StateMachine _gameStateMachine;

    private void Awake()
    {
        SwitchTransition endGameTransition = null;
        SwitchTransition endReplayTransition = null;
        
        IState startGame = new StartGame(() => endGameTransition!.Switch());
        IState wait = new StartWait();
        IState watchReplayState = new WatchReplayState(() => endReplayTransition!.Switch());
        
        endGameTransition = new SwitchTransition(startGame, wait);
        endReplayTransition = new SwitchTransition(watchReplayState, wait);
        
        _gameStateMachine = new StateMachine(wait);
        _gameStateMachine.AddTransition(new StartButtonTransition(wait, startGame));
        _gameStateMachine.AddTransition(endGameTransition);
        _gameStateMachine.AddTransition(new WatchReplayButtonTransition(wait, watchReplayState));
        _gameStateMachine.AddTransition(endReplayTransition);
    }
    
    private void Update()
    {
        _gameStateMachine.Tick();
        TickManager.Tick();
    }

    private void OnDisable()
    {
        TickManager.Dispose();
    }
}