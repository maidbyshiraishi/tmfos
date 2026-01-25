using Godot;
using tmfos.system;

namespace tmfos.command.decoration;

/// <summary>
/// フローティングメッセージを表示するコマンド
/// </summary>
public partial class ShowFloatingMessageCommand : CommandNode2D
{
    /// <summary>
    /// 表示するメッセージ
    /// </summary>
    [Export]
    public string Message { get; set; }

    /// <summary>
    /// 表示色
    /// </summary>
    [Export]
    public Color Color { get; set; } = Colors.White;

    public override void DoCommand(Node node, bool flag)
    {
        Lib.ShowFloatingMessage(this, Message, Color);
    }
}
