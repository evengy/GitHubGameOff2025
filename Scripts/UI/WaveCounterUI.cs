using System;
using TMPro;
using UnityEngine;

public class WaveCounterUI : MonoBehaviour
{
    public void AnimationCallback()
    {
        WaveStateManager.Instance.UpdateWaveState(WaveState.InWave);
    }

    public void UpdateWaveCounter()
    {
        GetComponent<Animator>().ResetTrigger("StartTransition");
        GetComponent<Animator>().SetTrigger("StartTransition");
    }
}