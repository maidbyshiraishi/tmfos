using Godot;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// ノードの物理演算を一時停止するコマンド
/// </summary>
public partial class PauseNodePhysicsProcessCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => node.SetPhysicsProcess(false);
}
