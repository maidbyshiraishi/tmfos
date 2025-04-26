using Godot;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 触手の頭
/// </summary>
public partial class TentacleHead : Area2D, IGameNode
{
    [Export]
    public float Speed { get; set; } = 3f;

    [Export]
    public float MaxLength { get; set; } = 300f;

    [Export]
    public double WaitTime { get; set; } = 3d;

    public Node2D MobPrevious { get; set; }
    public Node2D Root { get; set; }
    public Node2D Target { get; set; }

    private Player _player;
    private Timer _timer;

    public override void _Ready()
    {
        AddToGroup(StageRoot.GameNodeGroup);
        AddToGroup(StageRoot.PhysicsProcessGroup);
        _timer = GetNodeOrNull<Timer>("Timer");

        if (_timer is not null && 0.05f <= Mathf.Abs(WaitTime))
        {
            _timer.WaitTime = WaitTime;
            _ = _timer.Connect(Timer.SignalName.Timeout, new(this, MethodName.SetTargetPlayer));
        }
    }

    public void InitializeNode()
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        _player = stageRoot.GetNode<Player>("%Player");
        SetTargetPlayer();
    }

    public void FinalizeNode()
    {
    }

    public void RemoveNode()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = GlobalPosition.Lerp(Target.GlobalPosition, (float)delta * Speed);
        Position = Position.LimitLength(MaxLength);
    }

    public void Area2DEntered(Area2D area)
    {
        SetTargetRoot();
        Lib.ResetTimer(_timer);
    }

    public void SetTargetPlayer()
    {
        Target = _player;
    }

    private void SetTargetRoot()
    {
        Target = Root;
    }
}

