using Godot;
using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// ドリルショット
/// </summary>
public partial class DrillShot : Shot
{
    public override void _Ready()
    {
        base._Ready();
        PlaySprite();
    }

    protected override void PlaySpawnedSe()
    {
        GetNode<SePlayer>("/root/SePlayer").Play("drill_shot");
    }

    public override void CalcDirection()
    {
    }

    public override void SetNodeInfo(Vector2 position, Vector2 direction)
    {
        Position = position;
    }
}
