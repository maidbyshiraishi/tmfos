using Godot;
using tmfos.mob;

namespace tmfos.enemy;

/// <summary>
/// メイドロボボス第2段階
/// </summary>
public partial class MaidRobot2Enemy : Enemy
{
    [Export]
    public float ShotWait { get; set; } = 1.5f;

    [Export]
    public float JumpWait { get; set; } = 3f;

    [Export]
    public Node2D EntryPoints { get; set; }

    [Export]
    public Node2D DrillPoints { get; set; }

    private bool _lastOnFloor;
    private int _entryCount = 0;
    private int _drillCount = 0;
    private bool _active = false;
    private bool _jump = false;

    public override void _Ready()
    {
        base._Ready();
        GetNode<Timer>("DrillTimer").WaitTime = ShotWait;
        GetNode<Timer>("JumpTimer").WaitTime = JumpWait;
        _entryCount = EntryPoints is null ? 0 : EntryPoints.GetChildCount();
        _drillCount = DrillPoints is null ? 0 : DrillPoints.GetChildCount();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_active)
        {
            return;
        }

        bool onFloor = IsOnFloor();
        Vector2 velocity = Velocity;

        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            velocity.X = 0f;
            velocity.Y += m_gravity * (float)delta;
        }
        else
        {
            velocity.Y += m_gravity * (float)delta;

            if (onFloor && _jump)
            {
                velocity.Y = m_jumpVelocity;
                PlaySe(JumpSe);
                _jump = false;
            }
        }

        Velocity = velocity;
        _ = MoveAndSlide();

        if (onFloor && !_lastOnFloor)
        {
            PlaySe(TouchdownSe);
        }

        _lastOnFloor = onFloor;
    }

    public void ShotDrill()
    {
        if (!_active)
        {
            return;
        }

        RandomNumberGenerator random = new();
        DrillPoints.GetNode<EnemySpawner>($"DrillShot{random.RandiRange(1, _drillCount)}").SpawnEnemy();
    }

    public override void Damaged()
    {
        if (!_active)
        {
            return;
        }

        base.Damaged();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }

    public void Start()
    {
        _active = true;
        GetNode<TextureProgressBar>("%HUD/BossLife").MaxValue = Life;
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }

    public void Jump()
    {
        _jump = true;
    }

    public void Respawn()
    {
        if (!_active || !IsOnFloor())
        {
            return;
        }

        RandomNumberGenerator random = new();
        Marker2D point = EntryPoints.GetNode<Marker2D>(string.Format("Marker2D{0:#}", random.RandiRange(1, _entryCount)));
        Position = point.GlobalPosition;
    }
}
