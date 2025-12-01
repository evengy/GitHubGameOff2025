using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WavesOrchestrator : Singleton<WavesOrchestrator>, IResetable
{
    [SerializeField] private Link[] links;
    [SerializeField] private Waypoint[] terminals;

    [Header("Tutorial")]
    [SerializeField] Transform markers;
    [SerializeField] GameObject markerPrefab;
    [SerializeField] float markerInstantiatonPeriod = 0.2f;

    float tutorialTimer = 0;

    private void Start()
    {
        WaveStateManager.Instance.StateUpdated += Instance_StateUpdated;
    }

    public void ResetBackToDefault()
    {
        StopAllCoroutines();
    }

    private void Instance_StateUpdated()
    {
        if (WaveStateManager.Instance.WaveState.Equals(WaveState.InStartNewWave))
        {
            foreach (Link link in links)
            {
                link.Reset();
            }


            int wave = HUD.Instance.WaveCounter;
            int incrementalComplexity = 0;

            switch (wave)
            {
                #region presets
                case 1:
                    tutorialTimer = -2;
                    links[1].Ignite();
                    StartCoroutine(ShowPathCoroutine(links[1].Start));
                    break;
                case 2:
                    links[0].Ignite();
                    links[1].Ignite();
                    break;
                case 3:
                    links[0].Ignite();
                    links[2].Ignite();
                    break;
                case 4:
                    links[3].Ignite();
                    links[6].Ignite();
                    break;
                case 5:
                    links[1].Ignite();
                    links[6].IgniteStart();
                    links[5].Ignite();
                    break;
                case 6:
                    links[1].Ignite();
                    links[0].IgniteStart();
                    links[5].Ignite();
                    break;
                case 7:
                    links[2].Ignite();
                    links[3].Ignite();
                    break;
                case 8:
                    links[0].Ignite();
                    links[6].Ignite();
                    break;
                case 9:
                    links[4].IgniteStart();
                    links[0].Ignite();
                    links[2].Ignite();
                    break;
                #endregion
                #region late game
                case <= 16:
                    incrementalComplexity = 3;
                    break;
                case <= 24:
                    incrementalComplexity = 4;
                    break;
                case <= 32:
                    incrementalComplexity = 5;
                    break;
                case > 32:
                    incrementalComplexity = 6;
                    break;
                #endregion
            }

            // turn off one of the terminal points to reduce visual overload of the scene
            if (incrementalComplexity > 0)
            {
                List<int> all = new List<int>() { 1, 2, 3, 4, 5, 6, 0 };

                for (int i = 0; i < all.Count; i++)
                {
                    int j = Random.Range(i, all.Count);
                    (all[i], all[j]) = (all[j], all[i]);
                }

                List<int> picked = all.GetRange(0, incrementalComplexity);

                foreach (var index in picked)
                {
                    links[index].Ignite();
                }
                if (terminals.Count(x => x.IsIgnited) > 2) 
                {
                    links[picked[0]].Reset();
                    links[picked[0]].IgniteStart();
                }
            }
        }
    }

    // To be depricated, new tutorial to be implemented
    IEnumerator ShowPathCoroutine(Waypoint from)
    {
        Waypoint current = from;
        bool isMarkerFlipped = false;

        while (current != null)
        {
            tutorialTimer += Time.deltaTime;

            if (tutorialTimer > markerInstantiatonPeriod)
            {
                current = current.GetNextWaypoint();

                if (current != null)
                {
                    Waypoint next = current.GetNextWaypoint();
                    if (next != null && next.GetNextWaypoint() != null)
                    {
                        Vector3 atPosition = new Vector3(
                            current.transform.position.x,
                            current.transform.position.y + 0.3f,
                            current.transform.position.z);
                        Vector3 direction = (next.transform.position - current.transform.position).normalized;
                        direction.y = 0f;
                        Quaternion rotation = Quaternion.LookRotation(direction)
                            * Quaternion.Euler(-90, 180, 0); // blender export inconsistency

                        var marker = Instantiate(markerPrefab, atPosition, rotation);
                        if (isMarkerFlipped)
                        {
                            marker.GetComponent<WaypointMarker>().FlipMesh();
                        }
                        marker.transform.SetParent(markers);
                    }
                }

                tutorialTimer = 0;
                isMarkerFlipped = !isMarkerFlipped;
            }

            yield return null;
        }
    }

}