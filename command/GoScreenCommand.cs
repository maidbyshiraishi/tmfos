using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// 指定スクリーンへ移動するコマンド
/// </summary>
public partial class GoScreenCommand : CommandNode
{
    /// <summary>
    /// 次スクリーンのリソースパス
    /// </summary>
    [Export]
    public string Screen { get; set; }

    /// <summary>
    /// フェードアウトエフェクト
    /// </summary>
    [Export]
    public string Fadeout { get; set; } = ScreenFader.Fadeout1;

    /// <summary>
    /// フェードインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = ScreenFader.Fadein1;

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenScreen(Screen, Fadeout, Fadein);
    }
}
