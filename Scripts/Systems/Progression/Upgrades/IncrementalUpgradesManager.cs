using UnityEngine;

// TODO segregate
public class IncrementalUpgradesManager : Singleton<IncrementalUpgradesManager>
{
    [Header("Cloud Upgrades")]
    [SerializeField] private float cloudDefaultSpeed = 5f;
    [SerializeField] private float cloudMaxSpeed = 30;
    [SerializeField] private int cloudDefaultUpgradeCostSpeed = 2;
    [SerializeField] private float cloudDefaultSize = 1;
    [SerializeField] private float cloudMaxSize = 3;
    [SerializeField] private int cloudDefaultUpgradeCostSize = 2;

    public float CloudCurrentSpeed { get; private set; }
    public float CloudCurrentSize { get; private set; }
    public int CloudCurrentUpgradeCostSpeed { get; private set; }
    public int CloudCurrentUpgradeCostSize { get; private set; }

    [Header("Umbrella Upgrades")]
    [Range(0, 100)]
    [SerializeField] int umbrellaOpenDefaultProbability = 20;
    [SerializeField] int umbrellaOpenMaxProbability = 80;
    [SerializeField] int umbrellaOpenProbabilityDefaultUpgradeCost = 2;
    [SerializeField] int chanceIncrement = 5;
    public int UmbrellaOpenCurrentProbability { get; private set; }
    public int UmbrellaCurrentMaxProbability => umbrellaOpenMaxProbability;
    public int UmbrellaOpenCurrentProbabilityUpgradeCost { get; private set; }

    [Header("")]
    [SerializeField] float thunderUseTime = 1f;
    [SerializeField] float thunderCD = 10;
    public float ThunderUseTime => thunderUseTime;
    public float ThunderCD => thunderCD;
    public bool ThunderCurrentAutocast { get; private set; }

    [Header("")]
    [SerializeField] float stormUseTime = 15;
    [SerializeField] float stormMinCD = 20;
    [SerializeField] float stormDefaultCD = 30;
    [SerializeField] int stormDefaultCDUpgradeCost = 2;

    public float StormUseTime => stormUseTime;
    public float StormCurrentCD { get; private set; }
    public int StormCurrentCDUpgradeCost { get; private set; }
    public bool StormCurrentAutocast { get; private set; }


    [Header("")]
    [SerializeField] float yellowUseTime = 10;
    [SerializeField] float yellowMinCD = 15;
    [SerializeField] float yellowDefaultCD = 20;
    [SerializeField] float yellowDefaultSpeedMultiplier = 0.5f;
    [SerializeField] float yellowMinSpeedMultiplier = 0.1f;
    [SerializeField] int yellowDefaultCDUpgradeCost = 2;
    [SerializeField] int yellowDefaultSpeedMultiplierUpgradeCost = 2;
    public float YellowUseTime => yellowUseTime;
    public float YellowCurrentCD { get; private set; }
    public float YellowCurrentSpeedMultiplier { get; private set; }
    public int YellowCurrentUseTimeUpgradeCost { get; protected set; }
    public int YellowCurrentSpeedMultiplierUpgradeCost { get; private set; }



    [Header("")]
    [SerializeField] float orangeUseTime = 15;
    [SerializeField] float orangeMinCD = 20;
    [SerializeField] float orangeDefaultCD = 45;
    [SerializeField] int orangeDefaultCDUpgradeCost = 2;
    public float OrangeCurrentCD { get; private set; }
    public int OrangeCurrentCDUpgradeCost { get; private set; }
    public float OrangeUseTime => orangeUseTime;
    public bool OrangeCurrentAutocast { get; private set; }

    [Header("")]
    [SerializeField] float redUseTime = 5;
    [SerializeField] float redDefaultCD = 60;
    [SerializeField] float redMinCD = 10;
    [SerializeField] int redDefaultCDUpgradeCost = 2;
    public float RedUseTime => redUseTime;
    public float RedCurrentCD { get; private set; }
    public int RedCurrentCDUpgradeCost { get; private set; }
    public bool RedCurrentAutocast { get; private set; }

    public void Reset()
    {
        CloudCurrentSpeed = cloudDefaultSpeed;
        CloudCurrentSize = cloudDefaultSize;
        CloudCurrentUpgradeCostSpeed = cloudDefaultUpgradeCostSpeed;
        CloudCurrentUpgradeCostSize = cloudDefaultUpgradeCostSize;

        UmbrellaOpenCurrentProbability = umbrellaOpenDefaultProbability;
        UmbrellaOpenCurrentProbabilityUpgradeCost = umbrellaOpenProbabilityDefaultUpgradeCost;

        StormCurrentCD = stormDefaultCD;
        StormCurrentCDUpgradeCost = stormDefaultCDUpgradeCost;

        YellowCurrentCD = yellowDefaultCD;
        YellowCurrentSpeedMultiplier = yellowDefaultSpeedMultiplier;
        YellowCurrentUseTimeUpgradeCost = yellowDefaultCDUpgradeCost;
        YellowCurrentSpeedMultiplierUpgradeCost = yellowDefaultSpeedMultiplierUpgradeCost;

        OrangeCurrentCD = orangeDefaultCD;
        OrangeCurrentCDUpgradeCost = orangeDefaultCDUpgradeCost;

        RedCurrentCD = redDefaultCD;
        RedCurrentCDUpgradeCost = redDefaultCDUpgradeCost;

        ResetAllToggles();
    }

    private void Start()
    {
        Reset();
        GameStateManager.Instance.StateUpdated += Instance_StateUpdated;
    }

    private void Instance_StateUpdated()
    {
        if (GameStateManager.Instance.GameState.Equals(GameState.InGameOver))
        {
            Reset();
        }
    }
    public void ResetAllToggles()
    {
        ThunderCurrentAutocast = false;
        StormCurrentAutocast = false;
        OrangeCurrentAutocast = false;
        RedCurrentAutocast = false;
    }
    public string GetTooltip(IncrementalType upgradeIncrementalType)
    {
        switch (upgradeIncrementalType)
        {
            case IncrementalType.None:
                return "Rain princess";
            case IncrementalType.CloudSpeed:
                return $"Increase cloud speed : {CloudCurrentSpeed:F2} -> {CloudCurrentSpeed + 0.1f * (cloudMaxSpeed - CloudCurrentSpeed):F2}";
            case IncrementalType.CloudSize:
                return $"Increase cloud size :{CloudCurrentSize:F2} -> {CloudCurrentSize + 0.1f * (cloudMaxSize - CloudCurrentSize):F2}";
            case IncrementalType.OpenProbability:
                int increment = (UmbrellaOpenCurrentProbability < UmbrellaCurrentMaxProbability) ? chanceIncrement : 0;
                return $"Irish luck. Increases chance to open umbrella : {UmbrellaOpenCurrentProbability} -> {UmbrellaOpenCurrentProbability + increment}";
            case IncrementalType.StormCD:
                return $"Cooldown :{StormCurrentCD:F2} -> {StormCurrentCD - 0.1f * (StormCurrentCD - stormMinCD):F2}";
            case IncrementalType.YellowCD:
                return $"Cooldown :{YellowCurrentCD:F2} -> {YellowCurrentCD - 0.1f * (YellowCurrentCD - yellowMinCD):F2}";
            case IncrementalType.YellowSpeedMultiplier:
                return $"Stronger winds :{1/YellowCurrentSpeedMultiplier:F2} -> {1/(YellowCurrentSpeedMultiplier + 0.1f * (yellowMinSpeedMultiplier - YellowCurrentSpeedMultiplier)):F2}";
            case IncrementalType.OrangeCD:
                return $"Cooldown :{OrangeCurrentCD:F2} -> {OrangeCurrentCD - 0.1f * (OrangeCurrentCD - orangeMinCD):F2}";
            case IncrementalType.RedCD:
                return $"Cooldown :{RedCurrentCD:F2} -> {RedCurrentCD - 0.1f * (RedCurrentCD - redMinCD):F2}";
            case IncrementalType.ThunderToggle:
                string thunderAutocast = ThunderCurrentAutocast ? "On" : "Off";
                return $"Thunder autocast : {thunderAutocast}";
            case IncrementalType.StormToggle:
                string stormAutocast = StormCurrentAutocast ? "On" : "Off";
                return $"Storm autocast : {stormAutocast}";
            case IncrementalType.OrangeToggle:
                string orangeAutocast = OrangeCurrentAutocast ? "On" : "Off";
                return $"Orange warning autocast : {orangeAutocast}";
            case IncrementalType.RedToggle:
                string redAutocast = RedCurrentAutocast ? "On" : "Off";
                return $"Perfect storm autocast : {redAutocast}";
            default:
                break;
        }
        return string.Empty;
    }

    public bool Validate(IncrementalType upgradeIncrementalType)
    {
        switch (upgradeIncrementalType)
        {
            case IncrementalType.None:
                return false;
            case IncrementalType.OpenProbability:
                return UmbrellaOpenCurrentProbability < UmbrellaCurrentMaxProbability;
            default:
                break;
        }
        return false;
    }

    internal void Upgrade(IncrementalType upgradeIncrementalType)
    {
        switch (upgradeIncrementalType)
        {
            case IncrementalType.None:
                break;
            case IncrementalType.CloudSpeed:
                CloudCurrentUpgradeCostSpeed = CloudCurrentUpgradeCostSpeed * 2;
                CloudCurrentSpeed += 0.1f * (cloudMaxSpeed - CloudCurrentSpeed);
                break;
            case IncrementalType.CloudSize:
                CloudCurrentUpgradeCostSize = CloudCurrentUpgradeCostSize * 2;
                CloudCurrentSize += 0.1f * (cloudMaxSize - CloudCurrentSize);
                break;
            case IncrementalType.OpenProbability:
                int increment = (UmbrellaOpenCurrentProbability < UmbrellaCurrentMaxProbability) ? chanceIncrement : 0;
                if (increment > 0)
                {
                    UmbrellaOpenCurrentProbabilityUpgradeCost = UmbrellaOpenCurrentProbabilityUpgradeCost * 2;
                    UmbrellaOpenCurrentProbability += increment;
                }
                break;
            case IncrementalType.StormCD:
                StormCurrentCDUpgradeCost = StormCurrentCDUpgradeCost * 2;
                StormCurrentCD -= 0.1f * (StormCurrentCD - stormMinCD);
                break;
            case IncrementalType.YellowCD:
                YellowCurrentUseTimeUpgradeCost = YellowCurrentUseTimeUpgradeCost * 2;
                YellowCurrentCD -= 0.1f * (YellowCurrentCD - yellowMinCD);
                break;
            case IncrementalType.YellowSpeedMultiplier:
                YellowCurrentSpeedMultiplierUpgradeCost = YellowCurrentSpeedMultiplierUpgradeCost * 2;
                YellowCurrentSpeedMultiplier -= 0.1f * (YellowCurrentSpeedMultiplier - yellowMinSpeedMultiplier);
                break;
            case IncrementalType.OrangeCD:
                OrangeCurrentCDUpgradeCost = OrangeCurrentCDUpgradeCost * 2;
                OrangeCurrentCD -= 0.1f * (OrangeCurrentCD - orangeMinCD);
                break;
            case IncrementalType.RedCD:
                RedCurrentCDUpgradeCost = RedCurrentCDUpgradeCost * 2;
                RedCurrentCD -= 0.1f * (RedCurrentCD - redMinCD);
                break;
            case IncrementalType.ThunderToggle:
                ThunderCurrentAutocast = true;
                break;
            case IncrementalType.StormToggle:
                StormCurrentAutocast = true;
                break;
            case IncrementalType.OrangeToggle:
                OrangeCurrentAutocast = true;
                break;
            case IncrementalType.RedToggle:
                RedCurrentAutocast = true;
                break;
            default:
                break;
        }


    }

    internal int GetCost(IncrementalType upgradeIncrementalType)
    {
        switch (upgradeIncrementalType)
        {
            case IncrementalType.None:
                break;
            case IncrementalType.CloudSpeed:
                return CloudCurrentUpgradeCostSpeed;
            case IncrementalType.CloudSize:
                return CloudCurrentUpgradeCostSize;
            case IncrementalType.OpenProbability:
                return UmbrellaOpenCurrentProbabilityUpgradeCost;
            case IncrementalType.StormCD:
                return StormCurrentCDUpgradeCost;
            case IncrementalType.YellowCD:
                return YellowCurrentUseTimeUpgradeCost;
            case IncrementalType.YellowSpeedMultiplier:
                return YellowCurrentSpeedMultiplierUpgradeCost;
            case IncrementalType.OrangeCD:
                return OrangeCurrentCDUpgradeCost;
            case IncrementalType.RedCD:
                return RedCurrentCDUpgradeCost;
            default:
                break;
        }

        return 0;
    }
}