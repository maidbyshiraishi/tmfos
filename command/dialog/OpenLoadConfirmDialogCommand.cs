using Godot;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ロード確認ダイアログを開くコマンド
/// </summary>
public partial class OpenLoadConfirmDialogCommand : CommandNode
{
    /// <summary>
    /// ゲームデータ番号
    /// </summary>
    [Export]
    public int SlotNo { get; set; } = 0;

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/load_confirm_dialog.tscn", "LoadConfirmDialog", [SlotNo]);
    }
}
