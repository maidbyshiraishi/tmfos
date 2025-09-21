using Godot;
using tmfos.system;

namespace tmfos.mob;

/// <summary>
/// 耐久力のあるショット
/// </summary>
public partial class DurableShot : Shot, IDurable
{
    [Export]
    public double LifeTime { get; set; } = 0f;

    [Export]
    public int Life { get; set; } = 20;

    [Export]
    public int MaxLife { get; set; } = 20;

    [Export]
    public bool SkipDamage { get; set; } = false;

    [Export]
    public double SkipDamageTime { get; set; } = 0.2f;

    [Export]
    public int DamageBlinkFrame { get; set; } = 3;

    [Export]
    public MobStateType MobState { get; set; } = MobStateType.Normal;

    [Export]
    public string DeadSe { get; set; }

    [Export]
    public string DamageSe { get; set; }

    [Export]
    public string TimeupSe { get; set; }

    private Timer _lifeTimer;

    public override void _Ready()
    {
        base._Ready();
        _lifeTimer = GetNodeOrNull<Timer>("LifeTimer");

        if (_lifeTimer is not null && 0.05f <= Mathf.Abs(LifeTime))
        {
            _lifeTimer.WaitTime = LifeTime;
            _ = _lifeTimer.Connect(Timer.SignalName.Timeout, new(this, MethodName.Timeup));
        }
    }

    public override void InitializeNode()
    {
        Life = MaxLife;
        ResetLifeTime();
    }

    public double GetLifeTimeLeft()
    {
        return _lifeTimer.TimeLeft;
    }

    public virtual void Timeup()
    {
        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            return;
        }

        MobState = MobStateType.Timeup;
        PlaySprite("dead");
        PlaySe(TimeupSe);
    }

    public override void SetLifeTime(double lifeTime)
    {
        LifeTime = lifeTime;
    }

    public virtual void ResetLifeTime()
    {
        Lib.ResetTimer(_lifeTimer);
    }

    public virtual void AddDurability(int value)
    {
        // 連続で追加のダメージは受けない、
        // タイムアップ時は回復も負傷もしない
        if ((value <= 0 && (SkipDamage || MobState is MobStateType.Dead)) || (MobState is MobStateType.Timeup) || (MobState is MobStateType.Sleep))
        {
            return;
        }

        int oldLife = Life;
        Life = Mathf.Clamp(oldLife + value, 0, MaxLife);
        ManageState(oldLife, Life);

        if (value < 0)
        {
            Lib.ShowFloatingMessage(this, value.ToString(), Colors.Red);
        }
    }

    public void ManageState(int oldLife, int newLife)
    {
        switch (oldLife)
        {
            case 0 when newLife > 0:

                Resurrected();
                break;

            case > 0 when newLife == 0:

                Dead();
                break;

            case < 100 when newLife == 100:

                FullRecovered();
                break;

            default:

                if (oldLife < newLife)
                {
                    Recovered();
                }
                else if (newLife < oldLife)
                {
                    Damaged();
                }

                break;
        }
    }

    public virtual void Damaged()
    {
        SetSkipDamage();
        DamageBlink();
        PlaySe(DamageSe);
    }

    public virtual void Dead()
    {
        if (MobState is MobStateType.Dead)
        {
            return;
        }

        MobState = MobStateType.Dead;
        PlaySprite("dead");
        PlaySe(DeadSe);
    }

    public virtual void FullRecovered()
    {
    }

    public virtual void Recovered()
    {
    }

    public virtual void Resurrected()
    {
        MobState = MobStateType.Normal;
    }

    public virtual void SetDurability(int value)
    {
        // タイムアップ、スリープ時は回復も負傷もしない
        if (MobState is MobStateType.Timeup or MobStateType.Sleep)
        {
            return;
        }

        int oldLife = Life;
        Life = Mathf.Clamp(value, 0, MaxLife);
        ManageState(oldLife, Life);
    }

    protected async void DamageBlink()
    {
        for (int i = 0; i < DamageBlinkFrame; i++)
        {
            _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        while (SkipDamage)
        {
            m_animatedSprite.Hide();

            for (int i = 0; i < DamageBlinkFrame; i++)
            {
                _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }

            m_animatedSprite.Show();

            for (int i = 0; i < DamageBlinkFrame; i++)
            {
                _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }
        }

        m_animatedSprite.Show();
    }

    protected async void SetSkipDamage()
    {
        if (SkipDamage)
        {
            return;
        }

        if (SkipDamageTime >= 0.05f)
        {
            SkipDamage = true;
            _ = await ToSignal(GetTree().CreateTimer(SkipDamageTime), Timer.SignalName.Timeout);
        }

        SkipDamage = false;
    }

    public virtual void DieExternalCauses()
    {
        Dead();
    }
}
