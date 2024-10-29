using Godot;
using tmfos.mob;

namespace tmfos.enemy;

/// <summary>
/// メイドの出る蛇口ボス
/// </summary>
public partial class MaidFaucetEnemy : PathFollowEnemy
{
    /// <summary>
    /// 摘出現の間隔
    /// </summary>
    [Export]
    public double WaitTime { get; set; } = 3f;

    private EnemySpawner _spawnerLeft;
    private EnemySpawner _spawnerRight;

    public override void _Ready()
    {
        base._Ready();
        GetNode<Timer>("Timer").WaitTime = WaitTime;
        _spawnerLeft = GetNode<EnemySpawner>("EnemySpawner/EnemySpawnerLeft");
        _spawnerRight = GetNode<EnemySpawner>("EnemySpawner/EnemySpawnerRight");
        GetNode<TextureProgressBar>("%HUD/BossLife").MaxValue = Life;
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    public void SpawnEnemy()
    {
        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            return;
        }

        if (m_direction is DirectionType.Left or DirectionType.Up)
        {
            _spawnerLeft.SpawnEnemy();
        }
        else if (m_direction is DirectionType.Right or DirectionType.Down)
        {
            _spawnerRight.SpawnEnemy();
        }
    }

    public override void Damaged()
    {
        base.Damaged();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }
}
