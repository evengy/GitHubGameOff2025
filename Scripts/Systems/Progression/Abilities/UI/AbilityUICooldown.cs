using UnityEngine;
using UnityEngine.UI;

public class AbilityUICooldown : MonoBehaviour, IResetable
{
    [SerializeField] private Image cooldownOverlay;

    public bool IsCooldownActivated {  get; private set; }

    private IAbility ability;
    private float timer;

    private void Start()
    {
        ability = GetComponent<IAbility>();
    }

    private void Update()
    {
        if (IsCooldownActivated)
        {
            timer -= Time.deltaTime;
            cooldownOverlay.fillAmount = timer / ability.Cooldown();

            if (timer < 0)
            {
                IsCooldownActivated = false;
                cooldownOverlay.fillAmount = 0;
            }

            if (timer < ability.Cooldown() - ability.UseTime())
            {
                if (ability != null)
                {
                    ability.AbilityEnd();
                }
            }
        }
        // Handle Autocast
        else 
        {
            if (ability.IsAutocast())
            {
                ActivateCooldown();
            }
        }
    }

    public void ActivateCooldown()
    {
        cooldownOverlay.fillAmount = 1;
        IsCooldownActivated = true;
        timer = ability.Cooldown();

        if (ability != null)
        {
            ability.AbilityStart();
        }
    }

    public void ResetBackToDefault()
    {
        timer = 0;
    }
}