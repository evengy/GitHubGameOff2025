using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityUIContainer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private float scaleMultiplier = 1.2f;

    public UpgradeType UpgradeType => upgradeType;
    public bool IsAvailable { get; private set; }

    private AbilityUICooldown cooldown;
    private Animator animator;

    private void Start()
    {
        cooldown = GetComponent<AbilityUICooldown>();
        animator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.UseAbility();
        animator.ResetTrigger("AbilityHoverStart");
        animator.SetTrigger("AbilityHoverEnd");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ValidateIsAbilityAvailable() && !cooldown.IsCooldownActivated)
        {
            animator.ResetTrigger("AbilityHoverEnd");
            animator.SetTrigger("AbilityHoverStart");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.ResetTrigger("AbilityHoverStart");
        animator.SetTrigger("AbilityHoverEnd");
    }

    public void UseAbility()
    {
        if (ValidateIsAbilityAvailable())
        {
            cooldown.ActivateCooldown();
        }
    }

    private bool ValidateIsAbilityAvailable()
    {
        return !cooldown.IsCooldownActivated; // also check if upgrades UI is not opened 
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}