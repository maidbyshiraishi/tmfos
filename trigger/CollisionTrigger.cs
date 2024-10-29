using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// 接触時にコマンドを実行するトリガー
/// </summary>
public partial class CollisionTrigger : Node2D
{
    public void Exec(Node2D node)
    {
        Lib.ExecCommands(this, node, true);
    }

    public void ExecArea2D(Area2D node)
    {
        Lib.ExecCommands(this, node, true);
    }
}
