using Godot;

namespace maid_by_shiraishi.command;

/// <summary>
/// コマンドノード
/// </summary>
public partial class CommandRoot : Node, ICommand
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
