using Godot;
using tmfos.mob;
using static tmfos.enemy.EnemySpawner;

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
    public double WaitTime { get; set; } = 2f;

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

    public override void InitializeNode()
    {
        base.InitializeNode();
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
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

        RandomNumberGenerator random = new();
        EnemyResourceName enemyName = random.RandiRange(1, 8) == 1 ? EnemyResourceName.RocketMaidEnemy : EnemyResourceName.NekomimiMaidEnemy;

        if (m_direction is DirectionType.Left or DirectionType.Up)
        {
            _spawnerLeft.EnemyName = enemyName;
            _spawnerLeft.SpawnEnemy();
        }
        else if (m_direction is DirectionType.Right or DirectionType.Down)
        {
            _spawnerRight.EnemyName = enemyName;
            _spawnerRight.SpawnEnemy();
        }
    }

    public override void Dead()
    {
        base.Dead();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = 0;
    }

    public override void Damaged()
    {
        base.Damaged();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }
}
