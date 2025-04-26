using Godot;
using tmfos.enemy;
using tmfos.stage;
using tmfos.system;

namespace tmfos.mob;

/// <summary>
/// 弾の親
/// </summary>
public partial class Shot : Area2D, IGameNode, ISpawnedNode
{
    /// <summary>
    /// 弾速
    /// </summary>
    [Export]
    public float Speed { get; set; } = 600f;

    /// <summary>
    /// 攻撃力
    /// </summary>
    [Export]
    public int Attack { get; set; } = 20;

    [Export]
    public bool SkipAttack { get; set; } = false;

    [Export]
    public double SkipAttackTime { get; set; } = 0.0f;

    /// <summary>
    /// 進行方向
    /// </summary>
    [Export]
    public Vector2 Direction
    {
        get => _direction;
        set => _direction = value.Normalized();
    }

    /// <summary>
    /// 貫通
    /// </summary>
    [Export]
    public bool Penetration { get; set; } = false;

    [Export]
    public int Weapon { get; set; } = 0;

    protected VisibleOnScreenNotifier2D m_visibleOnScreenNotifier2D;
    protected AnimatedSprite2D m_animatedSprite;
    protected int m_attackCorrection = 0;

    private Vector2 _direction = Vector2.Right;
    private Timer _lifeTimer;

    public override void _Ready()
    {
        AddToGroup(StageRoot.GameNodeGroup);
        AddToGroup(StageRoot.PhysicsProcessGroup);
        m_animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        m_visibleOnScreenNotifier2D = GetNodeOrNull<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        _ = m_visibleOnScreenNotifier2D.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new(this, MethodName.PlaySpawnedSe));
        CalcDirection();
    }

    public virtual void InitializeNode()
    {
    }

    public void FinalizeNode()
    {
    }

    public virtual async void RemoveNode()
    {
        SetPhysicsProcess(false);
        GlobalPosition = new(-2000f, -2000f);

        for (int i = 0; i < 5; i++)
        {
            _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        QueueFree();
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 vector2 = _direction * (float)(Speed * delta);
        MoveLocalX(vector2.X);
        MoveLocalY(vector2.Y);
    }

    /// <summary>
    /// シーンに追加された際に発生する効果音
    /// </summary>
    protected virtual void PlaySpawnedSe()
    {
    }

    protected virtual void PlaySe(string name)
    {
        if (m_visibleOnScreenNotifier2D is null || m_visibleOnScreenNotifier2D.IsOnScreen())
        {
            GetNode<SePlayer>("/root/SePlayer").Play(name);
        }
    }

    public virtual void HitArea2D(Area2D node)
    {
        if (node is IDurable durable)
        {
            SetSkipAttack();
            durable.AddDurability(-Attack - Weapon - m_attackCorrection);
        }

        if (!Penetration)
        {
            RemoveNode();
        }
    }

    public virtual void HitNode2D(Node2D node)
    {
        if (node is IDurable durable)
        {
            SetSkipAttack();
            durable.AddDurability(-Attack - Weapon - m_attackCorrection);
        }

        if (!Penetration)
        {
            RemoveNode();
        }
    }

    protected async void SetSkipAttack()
    {
        if (SkipAttack)
        {
            return;
        }

        if (SkipAttackTime >= 0.05f)
        {
            SkipAttack = true;
            _ = await ToSignal(GetTree().CreateTimer(SkipAttackTime), Timer.SignalName.Timeout);
        }

        SkipAttack = false;
    }

    protected virtual void PlaySprite(string name = null)
    {
        if (m_animatedSprite is null || m_animatedSprite.SpriteFrames is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            m_animatedSprite.Play();
        }
        else if (m_animatedSprite.SpriteFrames.HasAnimation(name) && m_animatedSprite.Animation != name)
        {
            m_animatedSprite.Play(name);
        }
        else if (m_animatedSprite.SpriteFrames.HasAnimation("default"))
        {
            m_animatedSprite.Play("default");
        }
    }

    protected void PauseSprite()
    {
        m_animatedSprite.Pause();
    }

    public virtual void CalcDirection()
    {
        float rotation = _direction.Angle();
        CollisionShape2D collision = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

        if (collision is not null)
        {
            collision.Rotation = rotation;
        }

        VisibleOnScreenNotifier2D notifier = GetNodeOrNull<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

        if (notifier is not null)
        {
            notifier.Rotation = rotation;
        }

        m_animatedSprite.Rotation = rotation;
    }

    public void SetSpawner(ISpawner spawner)
    {
        _ = Connect(Node.SignalName.TreeExited, spawner.GetSignalMethod());
    }

    public virtual void SetNodeInfo(Vector2 position, Vector2 direction)
    {
        Position = position;
        Direction = direction;
    }

    public virtual void SetLifeTime(double lifeTime)
    {
    }
}
