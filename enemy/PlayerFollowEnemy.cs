using Godot;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// プレーヤーを追尾する敵
/// </summary>
public partial class PlayerFollowEnemy : Area2D, IGameNode, ISpawnedNode
{
    [Export]
    public Node2D ContainEnemy { get; set; }

    [Export]
    public float MaxSpeed { get; set; } = 250f;

    [Export]
    public float MinSpeed { get; set; } = 20f;

    [Export]
    public float ExploreSpeed { get; set; } = 180f;

    [Export]
    public float Acceleration { get; set; } = 1.2f;

    [Export]
    public float ReduceAcceleration { get; set; } = 0.5f;

    [Export]
    public float OutOfAngleReduceAcceleration { get; set; } = 0.2f;

    [Export]
    public float MaxDegAngle { get; set; } = 30f;

    [Export]
    public float IgnoreDegAngle { get; set; } = 5f;

    [Export]
    public double LifeTime { get; set; } = 0f;

    [Export]
    public double ExploreTime { get; set; } = 3f;

    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _startPosition;
    private Vector2 _oldPlayerPosition;
    private float _maxRadAngle;
    private float _ignoreRadAngle;
    private Vector2 _deadPosition;
    private bool _playerInSight = false;
    protected Player m_player;
    private Timer _exploreTimer;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.GameNodeGroup);
        AddToGroup(StageRoot.PhysicsProcessGroup);
        _exploreTimer = GetNode<Timer>("ExploreTimer");
        _ = _exploreTimer.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.ChangeExploreDirection));

        if (ContainEnemy is ISpawnedNode ispawn)
        {
            ispawn.SetLifeTime(LifeTime);
        }
    }

    public void InitializeNode()
    {
        if (ContainEnemy is not IGameNode inode)
        {
            RemoveNode();
            return;
        }

        _startPosition = Position;
        _maxRadAngle = Mathf.DegToRad(MaxDegAngle);
        _ignoreRadAngle = Mathf.DegToRad(IgnoreDegAngle);
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        m_player = stageRoot.GetNode<Player>("%Player");
        inode.InitializeNode();
        LostPlayer(null);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_playerInSight)
        {
            GlobalPosition += _velocity * (float)delta;
            return;
        }

        if (_velocity.Length() < MinSpeed)
        {
            _velocity = (m_player.GlobalPosition - GlobalPosition).Normalized() * MinSpeed;
            GlobalPosition += _velocity * (float)delta;
            return;
        }

        Vector2 target = m_player.GlobalPosition - GlobalPosition;
        float angle = _velocity.AngleTo(target);

        if (Mathf.Abs(angle) < _ignoreRadAngle)
        {
            _velocity *= Acceleration;
        }
        else if (Mathf.Abs(angle) < _maxRadAngle)
        {
            _velocity = _velocity.Rotated(angle);
            _velocity *= ReduceAcceleration;
        }
        else
        {
            angle = Mathf.Clamp(angle, -_maxRadAngle, _maxRadAngle);
            _velocity = _velocity.Rotated(angle);
            _velocity *= OutOfAngleReduceAcceleration;
        }

        _velocity = _velocity.LimitLength(MaxSpeed);
        GlobalPosition += _velocity * (float)delta;
    }

    public void FinalizeNode()
    {
    }

    public virtual async void RemoveNode()
    {
        SetPhysicsProcess(false);
        GlobalPosition = new Vector2(-2000, -2000);

        for (int i = 0; i < 5; i++)
        {
            _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        QueueFree();
    }

    public void SetLifeTime(double lifeTime)
    {
    }

    public void SetNodeInfo(Vector2 position, Vector2 direction)
    {
        Position = position;
    }

    public void SetSpawner(ISpawner spawner)
    {
        _ = Connect(Node.SignalName.TreeExited, spawner.GetSignalMethod());
    }

    public void FindPlayer(Node2D node)
    {
        _playerInSight = true;
        _exploreTimer.Stop();
    }

    public void LostPlayer(Node2D node)
    {
        _playerInSight = false;
        _velocity = Vector2.Left * ExploreSpeed;
        Lib.ResetTimer(_exploreTimer, ExploreTime);
    }

    public void ChangeExploreDirection()
    {
        _velocity *= -1f;
    }
}
