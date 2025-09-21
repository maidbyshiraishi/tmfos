namespace tmfos.mob;

/// <summary>
/// 耐久力のあるモノのインターフェース
/// </summary>
public interface IDurable
{
    void AddDurability(int value);

    void SetDurability(int value);

    void FullRecovered();

    void Recovered();

    void Damaged();

    void Dead();

    void Resurrected();
}
