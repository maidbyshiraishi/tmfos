using Godot;

namespace tmfos.command;

/// <summary>
/// Nodeを開放するコマンド
/// </summary>
public partial class FreeCommand : CommandNode
{
    /// <summary>
    /// 開放するノード
    /// </summary>
    [Export]
    public Node Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (Target is not null)
        {
            _ = Target.CallDeferred(Node.MethodName.QueueFree);
        }
    }
}
