using Godot;
using tmfos.player;

namespace tmfos.command;

/// <summary>
/// プレーヤーの残り時間をリセットするコマンド
/// </summary>
public partial class ResetPlayerLifeTimeCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is Player player)
        {
            player.ResetLifeTime();
        }
    }
}
