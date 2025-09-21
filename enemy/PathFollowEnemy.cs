using Godot;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// パスに沿って移動する敵
/// </summary>
public partial class PathFollowEnemy : DurableShot
{
    [Export]
    public string FlyingSe { get; set; }

    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _lastPosition;
    private Vector2 _deadPosition;
    private DirectionType _oldDirection = DirectionType.None;
    private CollisionShape2D _collision;
    private ReferenceRect _referenceLeftRight;
    private ReferenceRect _referenceUpDown;

    protected Player m_player;
    protected DirectionType m_direction = DirectionType.None;
    protected float m_gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");
        _referenceLeftRight = GetNodeOrNull<ReferenceRect>("ReferenceRect/ReferenceLeftRight");
        _referenceUpDown = GetNodeOrNull<ReferenceRect>("ReferenceRect/ReferenceUpDown");
    }

    public override void InitializeNode()
    {
        base.InitializeNode();
        _lastPosition = GlobalPosition;
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        m_player = stageRoot.GetNode<Player>("%Player");
        ResetLifeTime();
    }

    public override void _PhysicsProcess(double delta)
    {
        // 死亡時・タイムアップ時は落ちるだけ
        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            _velocity.X = 0f;
            _velocity.Y += m_gravity;
            _deadPosition += _velocity * (float)delta;
            GlobalPosition = _deadPosition;
            return;
        }

        Direction = GlobalPosition - _lastPosition;
        _lastPosition = GlobalPosition;
        UpdateDirection();
        PlaySprite();

        if (_oldDirection != m_direction)
        {
            PlaySe(FlyingSe);
            _oldDirection = m_direction;
        }
    }

    public override void RemoveNode()
    {
        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            base.RemoveNode();
        }
    }

    private void UpdateDirection()
    {
        ReferenceRect reference;

        if (Mathf.Abs(Direction.X) >= Mathf.Abs(Direction.Y))
        {
            //1
            if (Direction.X <= 0)
            {
                //a
                m_direction = DirectionType.Left;
                PlaySprite("left");
            }
            else
            {
                //b
                m_direction = DirectionType.Right;
                PlaySprite("right");
            }

            reference = _referenceLeftRight;
        }
        else
        {
            //2
            if (Direction.Y <= 0)
            {
                //a
                m_direction = DirectionType.Up;
                PlaySprite("up");
            }
            else
            {
                //b
                m_direction = DirectionType.Down;
                PlaySprite("down");
            }

            reference = _referenceUpDown;
        }

        Shape2D shape = _collision.Shape;

        if (reference is not null && shape is RectangleShape2D rect)
        {
            rect.Size = reference.Size;
            _collision.Position = reference.Position + reference.PivotOffset;
        }
    }

    public override void Dead()
    {
        if (MobState is MobStateType.Dead)
        {
            return;
        }

        base.Dead();
        _deadPosition = GlobalPosition;
        _velocity = Vector2.Zero;
        SetCollisionMaskValue(4, false);

        //画面外非表示で死んだ場合、即座に除去する
        if (!m_visibleOnScreenNotifier2D.IsOnScreen())
        {
            RemoveNode();
        }
    }

    public override void HitArea2D(Area2D node)
    {
    }

    public override void HitNode2D(Node2D node)
    {
        if (node is Player player)
        {
            player.AddDurability(-Attack - m_attackCorrection);
            BlinkCollision();
        }
    }

    private async void BlinkCollision()
    {
        _collision.SetDeferred("disabled", true);
        _ = await ToSignal(GetTree().CreateTimer(SkipAttackTime), Timer.SignalName.Timeout);
        _collision.SetDeferred("disabled", false);
    }

    protected override void PlaySpawnedSe()
    {
        GetNode<SePlayer>("/root/SePlayer").Play(FlyingSe);
    }

    public override void CalcDirection()
    {
    }

    public override void Timeup()
    {
        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            return;
        }

        base.Timeup();
        _deadPosition = GlobalPosition;
        _velocity = Vector2.Zero;
        SetCollisionMaskValue(4, false);
    }
}
