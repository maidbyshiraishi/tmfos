using Godot;
using tmfos.mob;

namespace tmfos.command.stage;

/// <summary>
/// 接触したIGameNodeを除去するコマンド
/// </summary>
public partial class RemoveIGameNodeCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is IGameNode inode)
        {
            inode.RemoveNode();
        }
    }
}
