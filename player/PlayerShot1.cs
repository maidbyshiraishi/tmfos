using maid_by_shiraishi.mob;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.player;

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


    public override void InitializeNode() =>
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
    protected override void PlaySpawnedSe() => GetNode<SePlayer>("/root/SePlayer").Play("player_shot1");
}
