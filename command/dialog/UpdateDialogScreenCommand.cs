using Godot;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ダイアログを再描画するコマンド
/// </summary>
public partial class UpdateDialogScreenCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").UpdateDialogScreen();
    }
}
