using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityRedWarning : MonoBehaviour, IAbility
{
    [SerializeField] private GameObject perfectStorm;
    [SerializeField] private float impulseDuration = 1f;
    [SerializeField] private float impulseForce = 0.2f;
    [SerializeField] private GameObject lightning;
   
    private InputSystem_Actions controls;

    private void Start()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();
        controls.Player.RedWarning.performed += Ability_performed;
    }

    private void Ability_performed(InputAction.CallbackContext obj)
    {
        if (GetComponent<AbilityUIContainer>().gameObject.activeSelf)
        {
            GetComponent<AbilityUIContainer>().UseAbility();
        }
    }
    public void AbilityStart()
    {
        GetComponent<AudioSource>()?.Play();
        perfectStorm.SetActive(true);

        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse.ImpulseDefinition.ImpulseDuration = impulseDuration;
        impulse.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;
        impulse.GenerateImpulseWithForce(impulseForce);

        lightning.SetActive(true);
    }
    
    public void AbilityEnd()
    {
        perfectStorm.SetActive(false);
    }

    public float UseTime()
    {
        return IncrementalUpgradesManager.Instance.RedUseTime;
    }

    public float Cooldown()
    {
        return IncrementalUpgradesManager.Instance.RedCurrentCD;
    }

    public bool IsAutocast()
    {
        return IncrementalUpgradesManager.Instance.RedCurrentAutocast;
    }
}