using Godot;
using tmfos.mob;

namespace tmfos.command;

/// <summary>
/// 耐久力を操作するコマンド
/// </summary>
public partial class AddDurabilityCommand : CommandNode
{
    /// <summary>
    /// 耐久力の増減
    /// </summary>
    [Export]
    public int Value { get; set; } = 20;

    public override void DoCommand(Node node, bool flag)
    {
        if (node is IDurable inode)
        {
            inode.AddDurability(Value);
        }
    }
}
