using TMPro;
using UnityEngine;

public class HUD : Singleton<HUD>, IUI, IResetable
{
    [SerializeField] private TMP_Text scoreValueLabel; // umbrellas opened total counter
    [SerializeField] private TMP_Text umbrellasOpenedLabel; // umbrellas opened in wave counter
    [SerializeField] private TMP_Text waveCounterValueLabel; // current wave

    [SerializeField] private GameObject upgradesNavigationUI; // UI container with upgrades navigation
    [SerializeField] private GameObject scoreContainer; // UI container with score points and icon
    [SerializeField] private GameObject waveContainer; // UI container with wave info UI - wave counter and umbrellas opened counter

    public int Score { get; private set; }
    public int WaveCounter { get; private set; }

    private int openedUmbrellasInWaveCounter;

    private void Start()
    {
        WaveStateManager.Instance.StateUpdated += Instance_StateUpdated;
    }

    private void Instance_StateUpdated()
    {
        switch (WaveStateManager.Instance.WaveState)
        {
            case WaveState.InWave:
                umbrellasOpenedLabel.enabled = true;
                waveCounterValueLabel.enabled = false;
                upgradesNavigationUI.SetActive(true);
                scoreContainer.SetActive(true);
                break;
            case WaveState.InStartNewWave:
                waveCounterValueLabel.enabled = true;
                UpdateWaveCounter();
                umbrellasOpenedLabel.enabled = false;
                scoreContainer.SetActive(false);
                upgradesNavigationUI.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void UpdateWaveCounter()
    {
        waveContainer.GetComponent<WaveCounterUI>().UpdateWaveCounter(); // update wave counter and callbaks to "InWave"
        
        openedUmbrellasInWaveCounter = 0; // reset umbrellas counter for the next wave
        WaveCounter++;
        waveCounterValueLabel.text = $"Wave {WaveCounter}";
    }

    public void UpdateScore()
    {
        Score++;
        scoreValueLabel.text = $"{Score}";
        umbrellasOpenedLabel.text = $"Wave {WaveCounter} \n{++openedUmbrellasInWaveCounter} / {WaveManager.Instance.MinionsInWave}";

        // TODO place animator on score - its not the HUD responsibility
        GetComponent<Animator>().ResetTrigger("Pop");
        GetComponent<Animator>().SetTrigger("Pop");
    }

    public void UseScoreForUpgrades(int upgradeCost)
    {
        Score -= upgradeCost;
        scoreValueLabel.text = $"{Score}";
    }

    public void UpdateUI(bool flag)
    {
        waveContainer.SetActive(flag);
        scoreContainer.SetActive(flag);
        upgradesNavigationUI.SetActive(flag);
    }

    public void ResetBackToDefault()
    {
        upgradesNavigationUI.GetComponent<IResetable>().ResetBackToDefault();
        Score = 0;
        WaveCounter = 0;
        scoreValueLabel.text = $"{Score}";
        waveCounterValueLabel.text = $"Wave {WaveCounter}";
    }
}