using System;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState GameState { get; private set; }

    public event Action StateUpdated;

    public void UpdateGameState(GameState newState)
    {
        GameState = newState;
        StateUpdated?.Invoke();
    }
}