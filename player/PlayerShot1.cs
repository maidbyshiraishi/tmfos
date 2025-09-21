using tmfos.mob;
using tmfos.system;

namespace tmfos.player;

/// <summary>
/// プレーヤー弾1
/// </summary>
public partial class PlayerShot1 : Shot
{
    public override void _Ready()
    {
        base._Ready();
        PlaySprite("default");
    }


    public override void InitializeNode()
    {
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
    }
    protected override void PlaySpawnedSe()
    {
        GetNode<SePlayer>("/root/SePlayer").Play("player_shot1");
    }
}
