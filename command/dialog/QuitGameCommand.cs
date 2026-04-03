using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ゲームを終了するコマンド
/// </summary>
public partial class QuitGameCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").QuitGame();
}
