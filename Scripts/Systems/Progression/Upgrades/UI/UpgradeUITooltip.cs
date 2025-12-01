using TMPro;
using UnityEngine;

public class UpgradeUITooltip : Singleton<UpgradeUITooltip>, IUI
{
    [SerializeField] private TMP_Text tooltipTextLabel;

    public void UpdateTooltipDescription(string newDescription)
    {
        tooltipTextLabel.text = newDescription;
    }

    public void UpdateUI(bool flag)
    {
        tooltipTextLabel.enabled = flag;
    }
}