namespace tmfos.mob;

/// <summary>
/// 耐久力のあるモノのインターフェース
/// </summary>
public interface IDurable
{
    public void AddDurability(int value);

    public void SetDurability(int value);

    public void FullRecovered();

    public void Recovered();

    public void Damaged();

    public void Dead();

    public void Resurrected();
}
