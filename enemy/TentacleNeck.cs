using Godot;
using tmfos.stage;

namespace tmfos.enemy;

/// <summary>
/// 触手の首
/// </summary>
public partial class TentacleNeck : CharacterBody2D
{
    public Node2D MobPrevious { get; set; }

    public Node2D MobSubsequent { get; set; }

    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        AddToGroup(StageRoot.PhysicsProcessGroup);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (MobPrevious is null || MobSubsequent is null)
        {
            return;
        }

        GlobalPosition = (MobPrevious.GlobalPosition + MobSubsequent.GlobalPosition) / 2f;
    }
}

