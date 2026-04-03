using Godot;
using maid_by_shiraishi.player;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// プレーヤーの残り時間をリセットするコマンド
/// </summary>
public partial class ResetPlayerLifeTimeCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is Player player)
        {
            player.ResetLifeTime();
        }
    }
}
