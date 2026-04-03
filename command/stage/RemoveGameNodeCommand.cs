using Godot;
using maid_by_shiraishi.mob;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// 接触したIGameNodeを除去するコマンド
/// </summary>
public partial class RemoveGameNodeCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is IGameNode inode)
        {
            inode.RemoveNode();
        }
    }
}
