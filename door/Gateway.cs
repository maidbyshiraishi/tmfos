using Godot;
using tmfos.command.stage;
using tmfos.mob;
using tmfos.trigger;

namespace tmfos.door;

/// <summary>
/// ステージ間移動の出入り口
/// </summary>
public partial class Gateway : TriggerArea2D
{
    /// <summary>
    /// 移動先ステージ番号
    /// </summary>
    [Export]
    public int DestStageNo { get; set; }

    /// <summary>
    /// 移動先ドア番号
    /// </summary>
    [Export]
    public int DestDoorNo { get; set; }

    /// <summary>
    /// ドア番号
    /// </summary>
    [Export]
    public int DoorNo { get; set; }

    [Export]
    public string Fadeout { get; set; } = "fadeout_1";

    [Export]
    public string Fadein { get; set; } = "fadein_1";

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
        ChangeStageCommand changeStageCommand = GetNode<ChangeStageCommand>("ChangeStageCommand");
        changeStageCommand.DestStageNo = DestStageNo;
        changeStageCommand.DestDoorNo = DestDoorNo;
        changeStageCommand.Fadein = Fadein;
        changeStageCommand.Fadeout = Fadeout;
    }
}
