using Godot;
using tmfos.enemy;
using tmfos.mob;
using tmfos.system;

namespace tmfos.decoration;

/// <summary>
/// 飾りエフェクト
/// </summary>
public partial class Decoration : Node2D, ISpawnedNode
{
    [Export]
    public string SeName { get; set; }

    public override void _Ready()
    {
        if (SeName is not null)
        {
            GetNode<SePlayer>("/root/SePlayer").Play(SeName);
        }
    }

    public void Finished()
    {
        QueueFree();
    }

    public void SetNodeInfo(Vector2 position, Vector2 direction)
    {
        Position = position;
    }

    public void SetLifeTime(double lifeTime)
    {
    }

    public void SetSpawner(ISpawner spawner)
    {
        _ = Connect(Node.SignalName.TreeExited, spawner.GetSignalMethod());
    }
}
