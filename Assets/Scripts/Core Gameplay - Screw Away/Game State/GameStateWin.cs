using System;
using UnityEngine;

public class GameStateWin : BaseGameState
{
    private GameStateMachine _gameStateMachine;

    public GameStateWin(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public override GameState GameState { get => GameState.Win; }

    public static event Action winLevelEvent;

    public override bool CanTransitionTo()
    {
        if (_gameStateMachine.CurrentState == GameState.Lose || _gameStateMachine.CurrentState == GameState.Win)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void Enter()
    {
        winLevelEvent?.Invoke();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
