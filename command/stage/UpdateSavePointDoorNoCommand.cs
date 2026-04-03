using Godot;
using maid_by_shiraishi.data;
using maid_by_shiraishi.door;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

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
            StageData stageData = GetNode<GameDataManager>("/root/GameDataManager").GetStageData();
            stageData.DoorNo = savePoint.DoorNo;
        }
    }
}
