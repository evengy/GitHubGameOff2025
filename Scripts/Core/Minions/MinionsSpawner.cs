using System.Linq;
using UnityEngine;

public class MinionsSpawner : MonoBehaviour
{
    [SerializeField] private Waypoint spawnPoint; 
    [SerializeField] private MinionController minionPrefab;
    [SerializeField] private float spawnDelayMin = 3f;
    [SerializeField] private float spawnDelayMax = 5f;
    private float timer;
    private float spawnDelay;

    private void Start()
    {
        WaveStateManager.Instance.StateUpdated += Instance_StateUpdated;
        ResetSpawnDelay();
    }

    private void Instance_StateUpdated()
    {
        if (WaveStateManager.Instance.WaveState.Equals(WaveState.InStartNewWave))
        {
            timer = -1;
        }
    }

    private void ResetSpawnDelay() =>
        spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

    private void Update()
    {
        if (!WaveStateManager.Instance.WaveState.Equals(WaveState.InWave)
            || !spawnPoint.IsIgnited)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnDelay && CanSpawn())
        {
            timer = 0f;
            SpawnMinion();
            ResetSpawnDelay();
        }
    }

    private bool CanSpawn()
    {
        return (WaveManager.Instance.MinionsSpawned < WaveManager.Instance.MinionsInWave)
            && WaveManager.Instance.GetActiveSpawnedMinionsCount() < WaveManager.Instance.GetMaxMinionsOnTheScene();
    }

    private void SpawnMinion()
    {
        MinionController minion = Instantiate(minionPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        if (minion != null)
        {
            minion.Init(spawnPoint);
            
            WaveManager.Instance.RegisterSpawnedMinion(minion);
        }
    }

    
}
