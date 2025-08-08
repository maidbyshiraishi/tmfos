using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 敵弾1
/// </summary>
public partial class EnemyShot1 : Shot
{
    public override void _Ready()
    {
        base._Ready();
        PlaySprite("default");
    }

    protected override void PlaySpawnedSe()
    {
        GetNode<SePlayer>("/root/SePlayer").Play("enemy_shot1");
    }
}
