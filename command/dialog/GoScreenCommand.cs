using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// 指定スクリーンへ移動するコマンド
/// </summary>
public partial class GoScreenCommand : CommandRoot
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

    public override void DoCommand(Node node, bool flag) => GetNode<DialogLayer>("/root/DialogLayer").OpenScreen(Screen, Fadeout, Fadein);
}
