using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityStorm : MonoBehaviour, IAbility
{
    [SerializeField] private GameObject storm;
    [SerializeField] private float impulseDuration = 1f;
    [SerializeField] private float impulseForce = 0.2f;
    
    private InputSystem_Actions controls;

    private void Start()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();
        controls.Player.Storm.performed += Ability_performed;
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
        storm.SetActive(true);

        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse.ImpulseDefinition.ImpulseDuration = impulseDuration;
        impulse.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulse.GenerateImpulseWithForce(impulseForce);
    }

    public void AbilityEnd() 
    {
        storm.SetActive(false);
    }

    public float UseTime()
    {
        return IncrementalUpgradesManager.Instance.StormUseTime;
    }

    public float Cooldown()
    {
        return IncrementalUpgradesManager.Instance.StormCurrentCD;
    }

    public bool IsAutocast()
    {
        return IncrementalUpgradesManager.Instance.StormCurrentAutocast;
    }
}