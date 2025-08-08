using Godot;
using tmfos.player;

namespace tmfos.command;

/// <summary>
/// プレーヤーの速度をゼロにするコマンド
/// </summary>
public partial class StopPlayerMoveCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is Player p)
        {
            p.Velocity = Vector2.Zero;
        }
    }
}
