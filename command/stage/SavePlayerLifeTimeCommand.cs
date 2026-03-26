using Godot;
using tmfos.data;
using tmfos.player;
using tmfos.system;

namespace tmfos.command.stage;

/// <summary>
/// プレーヤーの残り時間を保存するコマンド
/// </summary>
public partial class SavePlayerLifeTimeCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is not Player player)
        {
            return;
        }

        GameDataManager gdata = GetNode<GameDataManager>("/root/GameDataManager");
        StageData stageData = gdata.GetStageData();
        int stageNo = stageData.StageNo;

        if (0.05f <= player.LifeTime)
        {
            stageData.TakeOverStageNo = stageNo;
            stageData.TakeOverPlayerLifeTime = player.GetLifeTimeLeft();
        }
        else
        {
            stageData.TakeOverStageNo = stageNo;
            stageData.TakeOverPlayerLifeTime = 0f;
        }
    }
}
