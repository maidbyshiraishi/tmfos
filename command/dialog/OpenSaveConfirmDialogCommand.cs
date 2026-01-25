using Godot;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// セーブ確認ダイアログを開くコマンド
/// </summary>
public partial class OpenSaveConfirmDialogCommand : CommandNode
{
    /// <summary>
    /// セーブデータ番号
    /// </summary>
    [Export]
    public int SlotNo { get; set; } = 0;

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/save_confirm_dialog.tscn", "SaveConfirmDialog", [SlotNo]);
    }
}
