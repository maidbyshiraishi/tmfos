using Godot;
using maid_by_shiraishi.enemy;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.decoration;

/// <summary>
/// 飾りエフェクト
/// </summary>
public partial class Decoration : Node2D, ISpawnedNode
{
    [Export]
    public string SeName { get; set; }

    public override void _Ready()
    {
        if (GetNodeOrNull("AnimatedSprite2D") is AnimatedSprite2D animatedSprite2D)
        {
            animatedSprite2D.AnimationFinished += Finished;
        }

        if (SeName is not null)
        {
            GetNode<SePlayer>("/root/SePlayer").Play(SeName);
        }
    }

    public void Finished() => QueueFree();

    public void SetNodeInfo(Vector2 position, Vector2 direction) => Position = position;

    public void SetLifeTime(double lifeTime)
    {
    }

    public void SetSpawner(ISpawner spawner) => TreeExited += spawner.GetSignalMethod();
}
