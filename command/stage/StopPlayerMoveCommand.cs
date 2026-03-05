using Godot;
using tmfos.player;

namespace tmfos.command.stage;

/// <summary>
/// プレーヤーの速度をゼロにするコマンド
/// </summary>
public partial class StopPlayerMoveCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is Player p)
        {
            p.Velocity = Vector2.Zero;
        }
    }
}
