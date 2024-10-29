using Godot;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 触手の頭
/// </summary>
public partial class TentacleHead : Node2D, IGameNode
{
    [Export]
    public Node2D MobPrevious { get; set; }

    [Export]
    public Node2D Root { get; set; }

    [Export]
    public float Speed { get; set; } = 3f;

    [Export]
    public float MaxLength { get; set; } = 300f;

    [Export]
    public Node2D Target { get; set; }

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.GameNodeGroup);
        AddToGroup(StageRoot.PhysicsProcessGroup);
    }

    public void InitializeNode()
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        Target = stageRoot.GetNode<Player>("%Player");
    }

    public virtual void FinalizeNode()
    {
    }

    public virtual void RemoveNode()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = GlobalPosition.Lerp(Target.GlobalPosition, (float)delta * Speed);
        Position = Position.LimitLength(MaxLength);
    }
}

