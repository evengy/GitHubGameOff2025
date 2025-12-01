public class Upgrade
{
    public UpgradeType UpgradeType { get; private set; }

    public int Cost { get; private set; }
    public bool IsActivated { get; private set; }
    

    public Upgrade(int cost, UpgradeType upgradeType)
    {
        Cost = cost;
        UpgradeType = upgradeType;
    }

    public void Activate()
    {
        IsActivated = true;
    }

    public void Deactivate() 
    {
        IsActivated = false; 
    }
}