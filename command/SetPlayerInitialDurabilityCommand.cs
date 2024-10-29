using Godot;
using tmfos.data;
using tmfos.player;

namespace tmfos.command;

/// <summary>
/// プレイヤーの耐久力がデフォルト値未満の場合にデフォルト値まで回復するコマンド
/// </summary>
public partial class SetPlayerInitialDurabilityCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is Player player && player.Life < PlayerData.InitialLife)
        {
            player.SetDurability(PlayerData.InitialLife);
        }
    }
}
