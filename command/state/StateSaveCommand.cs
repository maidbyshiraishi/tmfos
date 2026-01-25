using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.state;

/// <summary>
/// ステージ情報を保存するコマンド
/// </summary>
public partial class StateSaveCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        stageRoot.StateSave();
    }
}
