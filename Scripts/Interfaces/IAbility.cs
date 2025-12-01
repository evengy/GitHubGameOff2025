public interface IAbility
{
    public void AbilityStart();

    public void AbilityEnd();

    public float UseTime();

    public float Cooldown();

    public bool IsAutocast();
}