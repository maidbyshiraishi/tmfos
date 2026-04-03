using Godot;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.player;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.enemy;

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
        AddToGroup(GameStageRoot.GameNodeGroup);
        AddToGroup(GameStageRoot.PhysicsProcessGroup);
        AreaEntered += Area2DEntered;

        if (GetNodeOrNull("Timer") is Timer timer && 0.05f <= Mathf.Abs(WaitTime))
        {
            _timer = timer;
            _timer.WaitTime = WaitTime;
            _timer.Timeout += SetTargetPlayer;
        }
    }

    public void InitializeNode()
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
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

    public void SetTargetPlayer() => Target = _player;

    private void SetTargetRoot() => Target = Root;
}

