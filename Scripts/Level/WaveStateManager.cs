using System;

public class WaveStateManager : Singleton<WaveStateManager>
{
    public WaveState WaveState { get; private set; }

    public event Action StateUpdated;

    private void Start()
    {
        GameStateManager.Instance.StateUpdated += Instance_StateUpdated;
    }

    private void Instance_StateUpdated()
    {
        if (GameStateManager.Instance.GameState.Equals(GameState.InGame))
        {
            UpdateWaveState(WaveState.InStartNewWave);
        }
        else
        {
            UpdateWaveState(WaveState.None);
        }
    }

    public void UpdateWaveState(WaveState newState)
    {
        WaveState = newState;
        StateUpdated?.Invoke();
    }
}