using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityThunder : MonoBehaviour, IAbility
{
    [SerializeField] Transform spawnedMinions;
    [SerializeField] private float impulseDuration = 0.5f;
    [SerializeField] private float impulseForce = 0.2f;
    [SerializeField] GameObject lightning;

    private InputSystem_Actions controls;

    private void Start()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();
        controls.Player.Thunder.performed += Ability_performed;
    }

    private void Ability_performed(InputAction.CallbackContext obj)
    {
        if (GetComponent<AbilityUIContainer>().gameObject.activeSelf)
        {
            GetComponent<AbilityUIContainer>().UseAbility();
        }
    }

    public void AbilityEnd()
    {
        lightning.SetActive(false);
    }
    public bool IsAutocast()
    {
        return IncrementalUpgradesManager.Instance.ThunderCurrentAutocast;
    }
    public void AbilityStart()
    {
        GetComponent<AudioSource>()?.Play();
        var impulse = GetComponent<CinemachineImpulseSource>();
        impulse.ImpulseDefinition.ImpulseDuration = impulseDuration;
        impulse.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;
        impulse.GenerateImpulseWithForce(impulseForce);

        lightning.SetActive(true);

        foreach (Transform child in spawnedMinions)
        {
            int rnd = Random.Range(0, 100);
            Debug.Log($"{rnd}, {IncrementalUpgradesManager.Instance.UmbrellaOpenCurrentProbability}");
            if (rnd < IncrementalUpgradesManager.Instance.UmbrellaOpenCurrentProbability)
            {
                child.gameObject.GetComponent<MinionController>().OpenUmbrella();
            }
        }
    }

    public float Cooldown()
    {
        return IncrementalUpgradesManager.Instance.ThunderCD;
    }

    public float UseTime()
    {
        return IncrementalUpgradesManager.Instance.ThunderUseTime;
    }
}