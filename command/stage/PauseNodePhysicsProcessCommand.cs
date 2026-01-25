using Godot;

namespace tmfos.command.stage;

/// <summary>
/// ノードの物理演算を一時停止するコマンド
/// </summary>
public partial class PauseNodePhysicsProcessCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        node.SetPhysicsProcess(false);
    }
}
