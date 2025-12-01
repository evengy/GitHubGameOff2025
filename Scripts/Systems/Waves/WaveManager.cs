using System;
using System.Linq;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>, IResetable
{
    [SerializeField] private int defaultMinionsInWave = 5;
    [SerializeField] private int defaultMaxMinionsOnTheScene = 5;
    [SerializeField] Transform spawnedMinions;

    public int MinionsInWave => minionsInWave;
    public int MinionsSpawned => minionsSpawnedCounter;
    
    private int minionsSpawnedCounter;
    private int minionsInWave;

    public int GetActiveSpawnedMinionsCount()
    {
        return spawnedMinions.GetComponentsInChildren<MinionController>().Count();
    }

    public int GetMaxMinionsOnTheScene()
    {
        return defaultMaxMinionsOnTheScene + HUD.Instance.WaveCounter % 2;
    }

    public void RegisterSpawnedMinion(MinionController minion)
    {
        minion.transform.SetParent(spawnedMinions);
        minion.OnFailure += (x) => Failed(minion);
        minion.OnSuccess += (x) => Succeed(minion);
    }

    public void ResetBackToDefault()
    {
        minionsSpawnedCounter = 0;
        minionsInWave = defaultMinionsInWave;
    }

    private void Succeed(MinionController minion)
    {
        Destroy(minion.gameObject);

        if (minionsSpawnedCounter >= minionsInWave
            && !spawnedMinions.GetComponentsInChildren<MinionController>().Any())
        {
            minionsSpawnedCounter = 0; // reset minions counter for the current
            minionsInWave++; // increase amount of minions in the next wave
            WaveStateManager.Instance.UpdateWaveState(WaveState.InStartNewWave);
        }
    }

    private void Failed(MinionController minion)
    {
        Destroy(minion.gameObject);
    }
}