using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ステージを変更するコマンド
/// </summary>
public partial class ChangeStageCommand : CommandNode
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
    public string Fadeout { get; set; } = ScreenFader.Fadeout1;

    /// <summary>
    /// フェイドインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = ScreenFader.Fadein1;

    public override void DoCommand(Node node, bool flag)
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        stageRoot.EnterGateway(DestStageNo, DestDoorNo, Fadeout, Fadein);
    }
}
