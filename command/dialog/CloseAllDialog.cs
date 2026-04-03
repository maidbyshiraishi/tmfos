using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログをすべて閉じるコマンド
/// </summary>
public partial class CloseAllDialog : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").CloseAllDialog();
}
