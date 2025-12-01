using System.Linq;
using UnityEngine;

public class AbilitiesUI : Singleton<AbilitiesUI>, IResetable
{
    [SerializeField] AbilityUIContainer[] abilities;

    public void SetAbilityToAvailableState(UpgradeType upgradeType)
    {
        abilities.FirstOrDefault(x => x.UpgradeType.Equals(upgradeType))?.gameObject.SetActive(true);
    }

    public bool IsActive<T>()
    {
        return abilities.FirstOrDefault(x => x.GetComponent<T>() != null)?.gameObject.activeSelf ?? false;
    }

    public void ResetBackToDefault()
    {
        foreach (var item in abilities)
        {
            item.GetComponent<AbilityUICooldown>().ResetBackToDefault();
            item.GetComponent<IAbility>().AbilityEnd();
            item.GetComponent<AbilityUIContainer>().Deactivate();
        }
    }
}