using Godot;
using tmfos.enemy;
using tmfos.mob;

namespace tmfos.decoration;

/// <summary>
/// フローティングメッセージ
/// </summary>
public partial class FloatingMessage : Marker2D, ISpawnedNode
{
    [Export]
    public string Text { get; set; } = "text";

    [Export]
    public Color Color { get; set; } = Colors.White;

    public override void _Ready()
    {
        GetNode<Label>("Label").Text = Text;
        GetNode<Label>("Label").SelfModulate = Color;
    }

    public void Finished(StringName animName)
    {
        QueueFree();
    }

    public void SetNodeInfo(Vector2 position, Vector2 direction, double lifeTime)
    {
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
