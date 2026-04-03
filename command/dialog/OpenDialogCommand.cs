using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログを開くコマンド
/// </summary>
public partial class OpenDialogCommand : CommandRoot
{
    /// <summary>
    /// 開くダイアログ
    /// </summary>
    [Export]
    public string DialogPath { get; set; }

    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").OpenDialog(DialogPath);
}
