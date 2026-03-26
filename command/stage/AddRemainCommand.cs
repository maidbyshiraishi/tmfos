using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.stage;

/// <summary>
/// 残機を増やすコマンド
/// </summary>
public partial class AddRemainCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        stageRoot.AddRemain();
    }
}
