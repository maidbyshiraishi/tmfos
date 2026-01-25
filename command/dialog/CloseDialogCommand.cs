using Godot;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ダイアログを閉じるコマンド
/// </summary>
public partial class CloseDialogCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").CloseDialog();
    }
}
