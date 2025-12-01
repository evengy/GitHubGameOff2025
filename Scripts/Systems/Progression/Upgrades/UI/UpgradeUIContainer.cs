using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is legacy class. It has started as upgrade container only, now also handles incremental upgrades with autocast abilities.
/// TODO segregate responsibilities.
/// </summary>
public class UpgradeUIContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private IncrementalType upgradeIncrementalType;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text costValueLabel;
    [SerializeField] private int cost;
    [SerializeField] private string description;
    [SerializeField] private AudioClip powerClip;
    [SerializeField] private UpgradeUIContainer[] incrementalUpgrades;
    [SerializeField] private UpgradeUIContainer[] toggles;
    [SerializeField] private Image toggleIcon;

    private Animator animator;
    private Upgrade upgrade;

    private void Start()
    {
        if (!upgradeType.Equals(UpgradeType.Incremental))
        {
            costValueLabel.text = $"{cost}";
        }
        else
        {
            costValueLabel.text = $"{IncrementalUpgradesManager.Instance.GetCost(upgradeIncrementalType)}";
        }
        upgrade = new Upgrade(cost, upgradeType);
        animator = GetComponent<Animator>();
    }

    public void Deactivate()
    {
        if (upgradeType.Equals(UpgradeType.Incremental))
        {
            costValueLabel.text = $"{IncrementalUpgradesManager.Instance.GetCost(upgradeIncrementalType)}";
            if (toggleIcon != null)
            {
                toggleIcon.gameObject.SetActive(false);
            }
        }
        if (animator != null)
        {
            animator.enabled = false;
            animator.enabled = true;
            animator.Rebind();
        }
        foreach (var inc in incrementalUpgrades)
        {
            inc.Deactivate();
            inc.gameObject.SetActive(false);
        }
        costValueLabel.gameObject.SetActive(true);

        upgrade?.Deactivate();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        icon.gameObject.transform.localScale = Vector3.one;

        if (!upgrade.IsActivated)
        {
            if (!upgradeType.Equals(UpgradeType.Incremental))
            {
                // handle classic upgrade
                if (HUD.Instance.Score >= cost)
                {
                    animator.ResetTrigger("UpgradeHoverStart");
                    animator.SetTrigger("UpgradeHoverEnd");
                    
                    foreach (var inc in incrementalUpgrades)
                    {
                        inc.gameObject.SetActive(true);
                    }
                    costValueLabel.gameObject.SetActive(false);

                    HUD.Instance.UseScoreForUpgrades(cost);
                    upgrade.Activate();
                    AbilitiesUI.Instance.SetAbilityToAvailableState(upgradeType);
                }
            }
            else
            {
                // handle incremental upgrade
                if (HUD.Instance.Score >= IncrementalUpgradesManager.Instance.GetCost(upgradeIncrementalType))
                {
                    animator.ResetTrigger("UpgradeHoverStart");
                    animator.SetTrigger("UpgradeHoverEnd");
                    
                    // handle autocast toggle
                    if (toggleIcon != null)
                    {
                        IncrementalUpgradesManager.Instance.ResetAllToggles();
                        foreach (var tg in toggles)
                        {
                            tg.toggleIcon.gameObject.SetActive(false);
                        }

                        toggleIcon.gameObject.SetActive(true);

                        if (powerClip != null)
                        {
                            AmbientManager.Instance.CrossfadeTo(powerClip);
                        }
                    }
                    // handle incremental part
                    else
                    {
                        HUD.Instance.UseScoreForUpgrades(IncrementalUpgradesManager.Instance.GetCost(upgradeIncrementalType));
                    }
                    IncrementalUpgradesManager.Instance.Upgrade(upgradeIncrementalType);
                    costValueLabel.text = $"{IncrementalUpgradesManager.Instance.GetCost(upgradeIncrementalType)}";
                    UpgradeUITooltip.Instance.UpdateTooltipDescription(IncrementalUpgradesManager.Instance.GetTooltip(upgradeIncrementalType));
                    
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.ResetTrigger("UpgradeHoverEnd");
        animator.SetTrigger("UpgradeHoverStart");

        if (!upgradeType.Equals(UpgradeType.Incremental))
        {
            UpgradeUITooltip.Instance.UpdateTooltipDescription(description);
        }
        else
        {
            UpgradeUITooltip.Instance.UpdateTooltipDescription(IncrementalUpgradesManager.Instance.GetTooltip(upgradeIncrementalType));
        }
        UpgradeUITooltip.Instance.UpdateUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.ResetTrigger("UpgradeHoverStart");
        animator.SetTrigger("UpgradeHoverEnd");
        UpgradeUITooltip.Instance.UpdateUI(false);
    }
}
