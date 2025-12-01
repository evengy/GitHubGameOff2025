using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UpgradesUIManager : Singleton<UpgradesUIManager>, IUI, IResetable
{
    [SerializeField] private GameObject[] upgradeUIObjects; // individual upgrades
    [SerializeField] private GameObject incrementalUIObjects; // UI container with incremental upgrades
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private float timeScaleWhenOpened = 0.3f;

    private DepthOfField dof;

    void Start()
    {
        globalVolume.profile.TryGet(out dof);
    }

    public void UpdateUI(bool flag)
    {
        foreach (var item in upgradeUIObjects)
        {
            item.SetActive(flag);
        }

        incrementalUIObjects.SetActive(flag);
        tooltip.SetActive(flag);

        dof.active = flag;
        dof.gaussianStart.value = flag ? 0 : 30;
        Time.timeScale = flag ? timeScaleWhenOpened : 1;
    }

    public void ResetBackToDefault()
    {
        foreach (var item in upgradeUIObjects)
        {
            item.GetComponent<UpgradeUIContainer>().Deactivate();
        }
    }
}