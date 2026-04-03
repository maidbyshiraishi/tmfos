using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログを閉じるコマンド
/// </summary>
public partial class CloseDialogCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").CloseDialog();
}
