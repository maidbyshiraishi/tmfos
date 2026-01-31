using Godot;

namespace tmfos.command;

/// <summary>
/// CanvasItemを隠すコマンド
/// </summary>
public partial class HideCanvasItemCommand : CommandRoot
{
    /// <summary>
    /// 隠すCanvasItem
    /// </summary>
    [Export]
    public CanvasItem Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.Hide();
    }
}
