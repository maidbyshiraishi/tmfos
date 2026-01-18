using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// 接触時にコマンドを実行するトリガー
/// </summary>
public partial class CollisionTrigger : Area2D
{
    public override void _Ready()
    {
        _ = Connect(Area2D.SignalName.AreaExited, new(this, MethodName.ExecExitArea2D));
        _ = Connect(Area2D.SignalName.BodyExited, new(this, MethodName.ExecExit));
    }

    public void Exec(Node2D node)
    {
        Lib.ExecCommands(this, node, true);
    }

    public void ExecArea2D(Area2D node)
    {
        Lib.ExecCommands(this, node, true);
    }

    public void ExecExit(Node2D node)
    {
        Lib.ExecCommands(this, node, false);
    }

    public void ExecExitArea2D(Area2D node)
    {
        Lib.ExecCommands(this, node, false);
    }
}
