using Godot;

namespace maid_by_shiraishi.command;

/// <summary>
/// Nodeを開放するコマンド
/// </summary>
public partial class QueueFreeCommand : CommandRoot
{
    /// <summary>
    /// 開放するノード
    /// </summary>
    [Export]
    public Node Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null)
        {
            return;
        }

        Target.QueueFree();
    }
}
