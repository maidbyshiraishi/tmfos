using Godot;
using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// メイドロボボス第1段階
/// </summary>
public partial class MaidRobot1Enemy : Enemy
{
    [Export]
    public float ShotWait { get; set; } = 1.5f;

    private Node2D _entryPoint;
    private Node2D _drillPoint;
    private bool _lastOnFloor;
    private int _entryCount = 0;
    private int _drillCount = 0;

    public override void _Ready()
    {
        base._Ready();
        _entryPoint = GetNode<Node2D>("%EntryPoint");
        _drillPoint = GetNode<Node2D>("%DrillPoint");
        _entryCount = _entryPoint is null ? 0 : _entryPoint.GetChildCount();
        _drillCount = _drillPoint is null ? 0 : _drillPoint.GetChildCount();
        GetNode<Timer>("DrillTimer").WaitTime = ShotWait;
        _ = GetNode<Timer>("DrillTimer").Connect(Timer.SignalName.Timeout, new(this, MethodName.ShotDrill));
        GetNode<TextureProgressBar>("%HUD/BossLife").MaxValue = Life;
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }

    public override void InitializeNode()
    {
        base.InitializeNode();
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
        Direction = Lib.GetLRDirection(Position, m_player.Position);
        ChangeSprite("walk", Direction);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            velocity.X = 0f;
            velocity.Y += m_gravity * (float)delta;
        }
        else
        {
            velocity = NormalAction(delta, velocity);
        }

        Velocity = velocity;
        _ = MoveAndSlide();

        if (DirectAttack("res://decoration/slash.tscn"))
        {
            if (MobState is MobStateType.Dead)
            {
                SetCollisionMaskValue(4, false);
            }
            else
            {
                Reverse();
            }
        }
    }

    private Vector2 NormalAction(double delta, Vector2 velocity)
    {
        bool onFloor = IsOnFloor();
        ChangeSprite("walk", Direction);

        // 重力の処理は常に行う
        velocity.Y += m_gravity * (float)delta;
        velocity = Walk(delta, velocity, onFloor);

        if (onFloor && !_lastOnFloor)
        {
            PlaySe(TouchdownSe);
        }

        _lastOnFloor = onFloor;
        return velocity;
    }

    public override void RemoveNode()
    {
        // 死亡時は除去される
        if (MobState is MobStateType.Dead || _entryPoint is null)
        {
            GetNode<MaidRobot2Enemy>("../MaidRobot2Enemy").Start();
            base.RemoveNode();
            return;
        }

        // 画面外に出たがまだ死んではいないので復帰させる
        RandomNumberGenerator random = new();
        Marker2D point = _entryPoint.GetNode<Marker2D>(string.Format("Marker2D{0:#}", random.RandiRange(1, _entryCount)));
        Position = point.GlobalPosition;
        Direction = Lib.GetLRDirection(Position, m_player.Position);
        ChangeSprite("walk", Direction);
    }

    public void ShotDrill()
    {
        RandomNumberGenerator random = new();

        for (int i = 0; i < 3; i++)
        {
            _drillPoint.GetNode<EnemySpawner>($"DrillShot{random.RandiRange(1, _drillCount)}").SpawnEnemy();
        }
    }

    public override void Dead()
    {
        base.Dead();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = 0;
    }

    public override void Damaged()
    {
        base.Damaged();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }
}
