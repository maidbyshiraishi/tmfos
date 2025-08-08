using Godot;
using tmfos.decoration;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 敵の親
/// </summary>
public partial class Enemy : DurableMob, ISpawnedNode
{
    protected Player m_player;
    protected int m_attackCorrection = 0;

    public override void InitializeNode()
    {
        base.InitializeNode();
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        m_player = stageRoot.GetNode<Player>("%Player");
        m_attackCorrection = stageRoot.EnemyAttackCorrection;
    }

    public override void Dead()
    {
        if (MobState is MobStateType.Dead)
        {
            return;
        }

        base.Dead();
        SetCollisionMaskValue(4, false);
    }

    public override void Timeup()
    {
        if (MobState is MobStateType.Timeup)
        {
            return;
        }

        base.Timeup();
        SetCollisionMaskValue(4, false);
    }

    protected Player GetCollidedPlayer()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);

            if (collision.GetCollider() is Player player)
            {
                return player;
            }
        }

        return null;
    }

    protected bool DirectAttack(string effect)
    {
        if (SkipAttack || GetCollidedPlayer() is not Player player)
        {
            return false;
        }

        SetSkipAttack();
        player.AddDurability(-Attack - m_attackCorrection);

        if (Lib.GetPackedScene<PackedScene>(effect) is PackedScene pack && pack.Instantiate() is Decoration decoration)
        {
            _ = EmitSignal(Mob.SignalName.NodeSpawned, decoration, this, Position, Vector2.Zero, 0f);
        }

        return true;
    }

    public void SetNodeInfo(Vector2 position, Vector2 direction)
    {
        Position = position;
    }

    public void SetLifeTime(double lifeTime)
    {
        LifeTime = lifeTime;
    }

    public void SetSpawner(ISpawner spawner)
    {
        _ = Connect(Node.SignalName.TreeExited, spawner.GetSignalMethod());
    }
}
