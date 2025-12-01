
// Sets visibility for other UI componenets based on the game state
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] AudioClip menuAmbience; // menu loop
    [SerializeField] AudioClip gameAmbience; // default loop
    [SerializeField] AudioClip gameOverASMR;
    private void Start()
    {
        GameStateManager.Instance.StateUpdated += Instance_StateUpdated;
    }

    private void Instance_StateUpdated()
    {
        switch (GameStateManager.Instance.GameState)
        {
            case GameState.InMenu:
                //HUD.Instance.UpdateUI(false);
                UpgradesUIManager.Instance.UpdateUI(false);
                MenuUIManager.Instance.ShowMenu();
                break;
            case GameState.InGame:
                MenuUIManager.Instance.UpdateUI(false);
                WaveManager.Instance.ResetBackToDefault();

                //HUD.Instance.UpdateUI(true);
                break;
            case GameState.InGameOver:
                AmbientManager.Instance.ResetBackToDefault();
                AmbientManager.Instance.PlayIntroThenLoop(gameOverASMR,menuAmbience);
                IncrementalUpgradesManager.Instance.Reset();  // Important to do this before upgrades reset because upgrades fetches cost
                UpgradesUIManager.Instance.ResetBackToDefault();
                AbilitiesUI.Instance.ResetBackToDefault();
                WavesOrchestrator.Instance.ResetBackToDefault();
                HUD.Instance.ResetBackToDefault();

                //HUD.Instance.UpdateUI(false);
                UpgradesUIManager.Instance.UpdateUI(false);
                MenuUIManager.Instance.ShowGameOver();
                break;
            default:
                break;
        }
    }
}