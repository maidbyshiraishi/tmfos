using Godot;
using tmfos.system;

namespace tmfos.command.decoration;

/// <summary>
/// フローティングメッセージを表示するコマンド
/// </summary>
public partial class ShowFloatingMessageCommand : Node2D, ICommand
{
    /// <summary>
    /// 実行フラグ
    /// </summary>
    [Export]
    public bool ExecFlag { get; set; } = true;

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

    public virtual void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag == flag)
        {
            Lib.ShowFloatingMessage(this, Message, Color);
        }
    }
}
