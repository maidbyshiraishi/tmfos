using Godot;
using tmfos.data;
using tmfos.door;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// セーブポイントにおけるゲーム再開時のドア番号を更新するコマンド
/// </summary>
public partial class UpdateSavePointDoorNoCommand : CommandNode
{
    /// <summary>
    /// 移動先ドア番号
    /// </summary>
    [Export]
    public int DestDoorNo { get; set; }

    [Export]
    public SavePoint ParentSavePoint { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (ParentSavePoint is null)
        {
            return;
        }

        StageData stageData = GetNode<GameData>("/root/GameData").GetStageData();
        stageData.DoorNo = ParentSavePoint.DoorNo;
    }
}
