using UnityEngine;

public class GameStatePlaying : BaseGameState
{
    public override GameState GameState { get => GameState.Playing; }

    public override bool CanTransitionTo()
    {
        return true;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}
