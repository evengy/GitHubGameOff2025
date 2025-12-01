using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Sets menu UI actions based on the game state
public class MenuUIManager : Singleton<MenuUIManager>, IUI
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button startNewGameButton;
    [SerializeField] private Button retryGameButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button aboutButton;
    [Header("Titles")]
    [SerializeField] private TMP_Text titleLabel; // main title
    [SerializeField] private TMP_Text gameOverLabel; // game over title
    [Header("Navigation Sections")]
    [SerializeField] private GameObject creditsContent; // section "credits"
    [SerializeField] private GameObject aboutContent; // section "about"

    private void Start()
    {
        startNewGameButton.onClick.AddListener(() => { GameStateManager.Instance.UpdateGameState(GameState.InGame); });
        retryGameButton.onClick.AddListener(() => { GameStateManager.Instance.UpdateGameState(GameState.InGame); });
        backToMenuButton.onClick.AddListener(() => { GameStateManager.Instance.UpdateGameState(GameState.InMenu); });
        creditsButton.onClick.AddListener(() => { ShowCredits(); });
        aboutButton?.onClick.AddListener(() => { ShowAbout(); });
    }

    private void ResetButtonsScale()
    {
        startNewGameButton.transform.localScale = Vector3.one;
        retryGameButton.transform.localScale = Vector3.one;
        creditsButton.transform.localScale = Vector3.one;
        aboutButton.transform.localScale = Vector3.one;
        backToMenuButton.transform.localScale = Vector3.one;
    }

    public void UpdateUI(bool flag)
    {
        startNewGameButton?.gameObject.SetActive(flag);
        creditsButton?.gameObject.SetActive(flag);
        exitGameButton?.gameObject.SetActive(flag);
        aboutButton?.gameObject.SetActive(flag);
        retryGameButton?.gameObject.SetActive(flag);
        backToMenuButton?.gameObject.SetActive(flag);
        titleLabel?.gameObject.SetActive(flag);
        gameOverLabel?.gameObject.SetActive(flag);
        creditsContent.SetActive(false);
        aboutContent.SetActive(false);
        ResetButtonsScale();
    }

    internal void ShowMenu()
    {
        UpdateUI(false);

        exitGameButton?.gameObject.SetActive(true);
        creditsButton?.gameObject.SetActive(true);
        aboutButton?.gameObject.SetActive(true);
        titleLabel?.gameObject.SetActive(true);
        startNewGameButton?.gameObject.SetActive(true);
    }

    internal void ShowGameOver()
    {
        UpdateUI(false);

        backToMenuButton?.gameObject.SetActive(true);
        aboutButton?.gameObject.SetActive(false);
        gameOverLabel?.gameObject.SetActive(true);
        retryGameButton?.gameObject.SetActive(true);
    }

    internal void ShowCredits()
    {
        UpdateUI(false);

        creditsContent.GetComponent<CreditsScroll>().ResetPosition();
        creditsContent.SetActive(true);
        backToMenuButton?.gameObject.SetActive(true);

    }

    internal void ShowAbout()
    {
        UpdateUI(false);

        backToMenuButton?.gameObject.SetActive(true);
        aboutContent.SetActive(true);
    }
}
