using Godot;
using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// エロメイドボス
/// </summary>
public partial class EroMaidEnemy : Enemy
{
    private Node2D _entryPoint;
    private bool _lastOnFloor;
    private int _entryCount = 0;
    private int _drillCount = 0;
    private bool _shot = false;
    private Marker2D _marker;

    public override void _Ready()
    {
        base._Ready();
        // シーン全体からノードを参照するときにグループを使う。
        // 自ノード下にあるグループのみを参照するようなことはできない。
        _entryPoint = GetNode<Node2D>("%EntryPoint");
        GetNode<TextureProgressBar>("%HUD/BossLife").MaxValue = Life;
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
        _marker = GetNode<Marker2D>("ExcitationtMarker2D");
        _entryCount = _entryPoint is null ? 0 : _entryPoint.GetChildCount();
    }

    public override void InitializeNode()
    {
        base.InitializeNode();
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            velocity.X = 0f;
            velocity.Y += m_gravity * (float)delta;
        }
        else if (_shot)
        {
            velocity = NormalAction(delta, velocity);
        }

        Velocity = velocity;
        _ = MoveAndSlide();

        if (DirectAttack("res://decoration/slash.tscn"))
        {
            if (MobState == MobStateType.Dead)
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
            base.RemoveNode();
            return;
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
        Warp();
    }

    private void Laser()
    {
        Vector2 shotDirection = m_player.GlobalPosition - GlobalPosition;

        if (Lib.GetPackedScene<PackedScene>("res://enemy/enemy_laser1.tscn") is PackedScene pack && pack.Instantiate() is Shot shot)
        {
            shot.Penetration = true;
            _ = EmitSignal(Mob.SignalName.NodeSpawned, shot, this, GlobalPosition, shotDirection, 0f);
        }
    }

    private void Shot()
    {
        if (SkipAttack || _shot)
        {
            return;
        }

        SetSkipAttack();

        if (Lib.GetPackedScene<PackedScene>("res://enemy/enemy_shot2.tscn") is PackedScene pack && pack.Instantiate() is Shot)
        {
            for (float i = 0f; i < 360f; i += 30f)
            {
                Vector2 shotDirection = Vector2.Right.Rotated(Mathf.DegToRad(i));
                Shot shot = pack.Instantiate() as Shot;
                shot.Penetration = true;
                _ = EmitSignal(Mob.SignalName.NodeSpawned, shot, this, GlobalPosition, shotDirection, 0f);
            }
        }
    }

    public void Warp()
    {
        Velocity = Vector2.Zero;
        RandomNumberGenerator random = new();
        Marker2D point = _entryPoint.GetNode<Marker2D>(string.Format("Marker2D{0:#}", random.RandiRange(1, _entryCount)));
        Position = point.GlobalPosition;
        Direction = Lib.GetLRDirection(Position, m_player.Position);
        _shot = random.RandiRange(1, 4) == 1;

        if (_shot)
        {
            PlaySprite("laser");

            if (Lib.GetPackedScene<PackedScene>("res://decoration/excitation.tscn") is PackedScene pack && pack.Instantiate() is Node decoration)
            {
                _ = EmitSignal(Mob.SignalName.NodeSpawned, decoration, this, _marker.GlobalPosition, Vector2.Zero, 0f);
                _ = decoration.Connect(Node.SignalName.TreeExited, new(this, MethodName.Laser));
            }
        }
        else
        {
            PlaySprite("shot");
        }
    }
}
