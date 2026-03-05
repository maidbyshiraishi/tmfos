using Godot;
using tmfos.data;
using tmfos.door;
using tmfos.system;

namespace tmfos.command.stage;

/// <summary>
/// セーブポイントにおけるゲーム再開時のドア番号を更新するコマンド
/// </summary>
public partial class UpdateSavePointDoorNoCommand : CommandRoot
{
    /// <summary>
    /// 移動先ドア番号
    /// </summary>
    [Export]
    public int DestDoorNo { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (GetParent() is SavePoint savePoint)
        {
            StageData stageData = GetNode<GameData>("/root/GameData").GetStageData();
            stageData.DoorNo = savePoint.DoorNo;
        }
    }
}
