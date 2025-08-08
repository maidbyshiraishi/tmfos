using Godot;
using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// ドリルショット大
/// </summary>
public partial class BigDrillShot : Shot
{
    public override void _Ready()
    {
        base._Ready();
        PlaySprite();
    }

    public override void InitializeNode()
    {
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
    }

    protected override void PlaySpawnedSe()
    {
        GetNode<SePlayer>("/root/SePlayer").Play("big_drill_shot");
    }

    public override void CalcDirection()
    {
    }

    public override void SetNodeInfo(Vector2 position, Vector2 direction)
    {
        Position = position;
    }
}
