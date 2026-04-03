using Godot;
using maid_by_shiraishi.data;
using maid_by_shiraishi.player;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// プレイヤーの耐久力がデフォルト値未満の場合にデフォルト値まで回復するコマンド
/// </summary>
public partial class SetPlayerInitialDurabilityCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        Node node2 = stageRoot.GetNode("%Player");

        if (node2 is Player player && player.Life < PlayerData.InitialLife)
        {
            player.SetDurability(PlayerData.InitialLife);
        }
    }
}
