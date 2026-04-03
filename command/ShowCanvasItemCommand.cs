using Godot;

namespace maid_by_shiraishi.command;

/// <summary>
/// CanvasItemを表示するコマンド
/// </summary>
public partial class ShowCanvasItemCommand : CommandRoot
{
    /// <summary>
    /// 対表示するCanvasItem
    /// </summary>
    [Export]
    public CanvasItem Target { get; set; }

    public override void DoCommand(Node node, bool flag) => Target?.Show();
}
