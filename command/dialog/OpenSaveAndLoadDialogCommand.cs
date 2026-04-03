using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// セーブとロードダイアログを開くコマンド
/// </summary>
public partial class OpenSaveAndLoadDialogCommand : OpenDialogCommand
{
    /// <summary>
    /// ドア番号
    /// </summary>
    [Export]
    public int DoorNo { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameDataManager>("/root/GameDataManager").GetStageData().DoorNo = DoorNo;
        base.DoCommand(node, true);
    }
}
