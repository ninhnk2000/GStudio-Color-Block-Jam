using UnityEngine;

public enum GameState
{
    Playing,
    Win,
    Lose
}

public abstract class BaseGameState
{
    public abstract GameState GameState { get; }
    public abstract bool CanTransitionTo();
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}
