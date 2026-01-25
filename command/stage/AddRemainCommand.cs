using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.stage;

/// <summary>
/// 残機を増やすコマンド
/// </summary>
public partial class AddRemainCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        stageRoot.AddRemain();
    }
}
