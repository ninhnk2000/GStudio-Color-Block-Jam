using System;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class GameStateMachine : MonoBehaviour
{
    private BaseGameState currentState;

    [SerializeField] private ScrewBoxesObserver screwBoxesObserver;

    [SerializeField] private float delayToEnableInputAfterRevived;

    public GameState CurrentState
    {
        get => currentState.GameState;
    }

    public ScrewBoxesObserver ScrewBoxesObserver
    {
        get => screwBoxesObserver;
    }

    public static event Action<bool> enableInputEvent;

    private void Awake()
    {
        LevelLoader.startLevelEvent += EnterPlayingState;
        BlockManager.winLevelEvent += EnterWinState;
        LevelTimeCounter.loseLevelEvent += EnterLoseState;
        TimingBomb.loseLevelEvent += EnterLoseState;
        RevivePopup.reviveEvent += OnRevived;

        ChangeState(new GameStatePlaying());
    }

    void OnDestroy()
    {
        LevelLoader.startLevelEvent -= EnterPlayingState;
        BlockManager.winLevelEvent -= EnterWinState;
        LevelTimeCounter.loseLevelEvent -= EnterLoseState;
        TimingBomb.loseLevelEvent -= EnterLoseState;
        RevivePopup.reviveEvent -= OnRevived;
    }

    // void Update()
    // {
    //     if (currentState != null)
    //     {
    //         currentState.Update();
    //     }
    // }

    public void ChangeState(BaseGameState newState)
    {
        if (!newState.CanTransitionTo())
        {
            return;
        }

        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter();
    }

    private void EnterPlayingState()
    {
        ChangeState(new GameStatePlaying());

        Tween.Delay(delayToEnableInputAfterRevived).OnComplete(() =>
        {
            enableInputEvent?.Invoke(true);
        });
    }

    private void EnterWinState()
    {
        ChangeState(new GameStateWin(this));
    }

    private void EnterLoseState()
    {
        enableInputEvent?.Invoke(false);

        ChangeState(new GameStateLose(this));
    }

    private void OnRevived(BoosterType boosterType)
    {
        EnterPlayingState();
    }
}
