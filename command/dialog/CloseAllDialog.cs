using Godot;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ダイアログをすべて閉じるコマンド
/// </summary>
public partial class CloseAllDialog : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").CloseAllDialog();
    }
}
