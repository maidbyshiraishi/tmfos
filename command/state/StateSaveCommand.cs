using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.state;

/// <summary>
/// ステージ情報を保存するコマンド
/// </summary>
public partial class StateSaveCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        stageRoot.StateSave();
    }
}
