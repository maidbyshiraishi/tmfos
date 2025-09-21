using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ダイアログを開くコマンド
/// </summary>
public partial class OpenDialogCommand : CommandNode
{
    /// <summary>
    /// 開くダイアログ
    /// </summary>
    [Export]
    public string DialogPath { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog(DialogPath);
    }
}
