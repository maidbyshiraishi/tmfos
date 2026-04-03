using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ロード確認ダイアログを開くコマンド
/// </summary>
public partial class OpenLoadConfirmDialogCommand : CommandRoot
{
    /// <summary>
    /// ゲームデータ番号
    /// </summary>
    [Export]
    public int SlotNo { get; set; } = 0;

    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/load_confirm_dialog.tscn", "LoadConfirmDialog", [SlotNo]);
}
