using System;
using UnityEngine;
using static GameEnum;

public class GameStateLose : BaseGameState
{
    private GameStateMachine _gameStateMachine;

    public GameStateLose(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public override GameState GameState { get => GameState.Lose; }

    public static event Action loseLevelEvent;
    public static event Action<BoosterType> showRevivePopupEvent;

    public override bool CanTransitionTo()
    {
        if (_gameStateMachine.CurrentState == GameState.Win || _gameStateMachine.CurrentState == GameState.Lose)
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
        showRevivePopupEvent?.Invoke(BoosterType.FreezeTime);

        // if (_gameStateMachine.ScrewBoxesObserver.NumLockedScrewBoxes == 0)
        // {
        //     // loseLevelEvent?.Invoke();
        //     showRevivePopupEvent?.Invoke(BoosterType.ClearScrewPorts);
        // }
        // else
        // {
        //     showRevivePopupEvent?.Invoke(BoosterType.UnlockScrewBox);
        // }
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
