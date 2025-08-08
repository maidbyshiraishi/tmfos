using Godot;

namespace tmfos.command;

/// <summary>
/// コマンドノード
/// </summary>
public partial class CommandNode : Node, ICommand
{
    /// <summary>
    /// 実行フラグ
    /// </summary>
    [Export]
    public bool ExecFlag { get; set; } = true;

    public virtual void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag == flag)
        {
            DoCommand(node, flag);
        }
    }

    public virtual void DoCommand(Node node, bool flag)
    {
    }
}
