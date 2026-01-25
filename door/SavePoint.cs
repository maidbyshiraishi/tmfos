using Godot;
using tmfos.command.dialog;
using tmfos.command.stage;
using tmfos.mob;
using tmfos.trigger;

namespace tmfos.door;

/// <summary>
/// セーブポイント
/// </summary>
public partial class SavePoint : TriggerArea2D
{
    /// <summary>
    /// ドア番号
    /// </summary>
    [Export]
    public int DoorNo { get; set; }

    /// <summary>
    /// 出現時プレーヤー方向
    /// </summary>
    [Export]
    public DirectionType Direction { get; set; } = DirectionType.Left;

    public override void _Ready()
    {
        base._Ready();
        StageEntryPoint stateEntryPoint = GetNode<StageEntryPoint>("StageEntryPoint");
        stateEntryPoint.DoorNo = DoorNo;
        stateEntryPoint.Direction = Direction;
        OpenSaveAndLoadDialogCommand openSaveAndLoadDialogCommand = GetNode<OpenSaveAndLoadDialogCommand>("OpenSaveAndLoadDialogCommand");
        openSaveAndLoadDialogCommand.DoorNo = DoorNo;
    }
}
