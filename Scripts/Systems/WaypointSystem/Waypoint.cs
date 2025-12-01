using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint nextWaypoint;
    [SerializeField] private Waypoint terminalWaypoint;

    public bool IsIgnited { get; private set; }

    public void Ignite()
    {
        IsIgnited = true;
    }

    public void Reset()
    {
        IsIgnited = false;
    }

    public Waypoint GetNextWaypoint()
    {
        if (terminalWaypoint?.IsIgnited ?? false)
        {
            return terminalWaypoint;
        }
        return nextWaypoint;
    }
}