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
    public string Fadeout { get; set; } = "fadeout_1";

    /// <summary>
    /// フェードインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = "fadein_1";

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenScreen(Screen, Fadeout, Fadein);
    }
}
