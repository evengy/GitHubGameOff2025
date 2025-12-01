using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityYellowWarning : Singleton<AbilityYellowWarning>, IAbility
{
    [SerializeField] private GameObject wind;
    [SerializeField] private float impulseDuration = 10f;
    [SerializeField] private float impulseForce = 0.1f;

    private InputSystem_Actions controls;

    private void Start()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();
        controls.Player.YellowWarning.performed += Ability_performed;
    }

    private void Ability_performed(InputAction.CallbackContext obj)
    {
        if (GetComponent<AbilityUIContainer>().gameObject.activeSelf)
        {
            GetComponent<AbilityUIContainer>().UseAbility();
        }
    }

    public bool IsActive { get; private set; }

    public void AbilityStart()
    {
        GetComponent<AudioSource>().Play();
        IsActive = true;
        wind.SetActive(IsActive);
        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse.ImpulseDefinition.ImpulseDuration = impulseDuration;
        impulse.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;
        impulse.GenerateImpulseWithForce(impulseForce);
    }

    public void AbilityEnd()
    {
        IsActive = false;
        wind.SetActive(IsActive);
        GetComponent<AudioSource>().Stop();
    }

    public float UseTime()
    {
        return IncrementalUpgradesManager.Instance.YellowUseTime;
    }

    public float Cooldown()
    {
        return IncrementalUpgradesManager.Instance.YellowCurrentCD;
    }

    public float SpeedMultiplier()
    {
        return IncrementalUpgradesManager.Instance.YellowCurrentSpeedMultiplier;    
    }

    public bool IsAutocast()
    {
        return false;
    }
}