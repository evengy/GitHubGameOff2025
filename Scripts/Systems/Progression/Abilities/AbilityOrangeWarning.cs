using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityOrangeWarning : Singleton<AbilityOrangeWarning>, IAbility
{
    [SerializeField] private float impulseDuration = 1f;
    [SerializeField] private float impulseForce = 0.2f;

    public bool IsActive { get; private set; }

    private InputSystem_Actions controls;

    private void Start()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();
        controls.Player.OrangeWarning.performed += Ability_performed;
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
        IsActive = true;

        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse.ImpulseDefinition.ImpulseDuration = impulseDuration;
        impulse.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulse.GenerateImpulseWithForce(impulseForce);
    }

    public void AbilityEnd()
    {
        IsActive = false;
    }

    public float UseTime()
    {
        return IncrementalUpgradesManager.Instance.OrangeUseTime;
    }

    public float Cooldown()
    {
        return IncrementalUpgradesManager.Instance.OrangeCurrentCD;
    }

    public bool IsAutocast()
    {
        return IncrementalUpgradesManager.Instance.OrangeCurrentAutocast;
    }
}