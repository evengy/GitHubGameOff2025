using System;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    [SerializeField] private UmbrellaController umbrella;
    [SerializeField] private GameObject preGameoverHighlight;
    [SerializeField] private float defaultSpeed = 2f;
    [SerializeField] private float proximityThreshold = 0.1f;

    public Action<MinionController> OnFailure;
    public Action<MinionController> OnSuccess;

    private Waypoint currentWaypoint;
    private float speed;

    public void Init(Waypoint spawnPoint)
    {
        currentWaypoint = spawnPoint;

        var speedAdjustment = defaultSpeed + (0.1f * HUD.Instance.WaveCounter);
        speed = speedAdjustment > 10 ? 10 : speedAdjustment;
    }

    public void OpenUmbrella()
    {
        umbrella.Open();
        umbrella.GetComponent<Animator>().SetTrigger("Open");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rain"))
        {
            OpenUmbrella();
        }
    }

    private void Update()
    {
        // validate
        if (!WaveStateManager.Instance.WaveState.Equals(WaveState.InWave))
        {
            OnFailure = null;
            OnSuccess = null;
            Destroy(gameObject);
            return;
        }
        if (currentWaypoint == null) return;

        // handle movement
        Vector3 currentTarget = currentWaypoint.transform.position;
        Vector3 direction = (currentTarget - transform.position).normalized;

        transform.position += direction * speed * GetSpeedMultiplier() * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        var distanceToCurrentTarget = Vector3.Distance(transform.position, currentTarget);

        // handle pregameover proximity
        if (currentWaypoint.GetNextWaypoint() == null && !umbrella.IsOpened)
        {
            preGameoverHighlight.SetActive(true);
            preGameoverHighlight.transform.localScale = new Vector3(distanceToCurrentTarget * 0.7f, distanceToCurrentTarget * 0.7f, 0.05f);
        }
        else
        {
            preGameoverHighlight.SetActive(false);
        }

        // handle minion proximity
        if (distanceToCurrentTarget < proximityThreshold)
        {
            currentWaypoint = currentWaypoint.GetNextWaypoint();

            if (currentWaypoint == null)
            {
                if (!umbrella.IsOpened)
                {
                    GameStateManager.Instance.UpdateGameState(GameState.InGameOver);
                    OnFailure?.Invoke(this);
                }
                else
                {
                    OnSuccess?.Invoke(this);
                }
            }
        }
    }

    private float GetSpeedMultiplier()
    {
        if (AbilitiesUI.Instance.IsActive<AbilityYellowWarning>() && AbilityYellowWarning.Instance.IsActive)
        {
            return AbilityYellowWarning.Instance.SpeedMultiplier();
        }
        return 1;
    }
}
