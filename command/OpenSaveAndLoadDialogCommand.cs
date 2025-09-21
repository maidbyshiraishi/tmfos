using Godot;
using tmfos.system;

namespace tmfos.command;

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
        GetNode<GameData>("/root/GameData").GetStageData().DoorNo = DoorNo;
        base.DoCommand(node, true);
    }
}
