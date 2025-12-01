using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO fix props
public class UpgradesNavigationUI : MonoBehaviour, IResetable
{
    [SerializeField] private Button navigationUpgradesCloseButton;
    [SerializeField] private Button navigationUpgradesOpenButton;
    [SerializeField] private TMP_Text navigationStateLabel;
    [SerializeField] private Color upgradesLabelOpenColor; // #B3E7FF
    [SerializeField] private Color upgradesLabelCloseColor; // #D64672

    private string upgradesOpenLabelText = "Forecast";
    private string upgradesCloseLabelText = "Close";

    public void ResetBackToDefault()
    {
        navigationUpgradesCloseButton.gameObject.SetActive(false);
        navigationUpgradesOpenButton.gameObject.SetActive(true);
        UpgradesUIManager.Instance.UpdateUI(false);
        navigationStateLabel.text = "Forecast";
        navigationUpgradesCloseButton.transform.localScale = Vector3.one;
        navigationUpgradesOpenButton.transform.localScale = Vector3.one;
    }

    public void ExecuteOpen()
    {
        navigationUpgradesOpenButton.gameObject.SetActive(false);
        navigationUpgradesCloseButton.gameObject.SetActive(true);
        UpgradesUIManager.Instance.UpdateUI(true);
        navigationStateLabel.text = "Close";
        navigationStateLabel.color  = new Color(0.8396226f, 0.2732734f, 0.4464157f, 1); // upgradesLabelOpenColor; // new Color(0.8396226f, 0.2732734f, 0.4464157f, 1);
    }

    public void ExecuteClose()
    {
        navigationUpgradesOpenButton.gameObject.SetActive(true);
        navigationUpgradesCloseButton.gameObject.SetActive(false);
        UpgradesUIManager.Instance.UpdateUI(false);
        navigationStateLabel.text = "Forecast";
        navigationStateLabel.color = new Color(0.7028302f, 0.9053788f, 1, 1); // upgradesLabelCloseColor; // new Color(0.7028302f, 0.9053788f, 1, 1);
    }
}
