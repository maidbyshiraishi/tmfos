using Godot;
using tmfos.data;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.stage;

/// <summary>
/// プレイヤーの耐久力がデフォルト値未満の場合にデフォルト値まで回復するコマンド
/// </summary>
public partial class SetPlayerInitialDurabilityCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        Node node2 = stageRoot.GetNode("%Player");

        if (node2 is Player player && player.Life < PlayerData.InitialLife)
        {
            player.SetDurability(PlayerData.InitialLife);
        }
    }
}
