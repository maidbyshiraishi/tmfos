using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// ステージを変更するコマンド
/// </summary>
public partial class ChangeStageCommand : CommandRoot
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
    /// フェイドアウトエフェクト
    /// </summary>
    [Export]
    public string Fadeout { get; set; } = "fadeout_1";

    /// <summary>
    /// フェイドインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = "fadein_1";

    public override void DoCommand(Node node, bool flag)
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        stageRoot.EnterGateway(DestStageNo, DestDoorNo, Fadeout, Fadein);
    }
}
