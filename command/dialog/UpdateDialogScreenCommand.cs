using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログを再描画するコマンド
/// </summary>
public partial class UpdateDialogScreenCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").UpdateDialogScreen();
}
