using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.state;

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
