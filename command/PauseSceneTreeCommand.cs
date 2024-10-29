using Godot;

namespace tmfos.command;

/// <summary>
/// ゲームを一時停止するコマンド
/// </summary>
public partial class PauseSceneTreeCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetTree().Paused = true;
    }
}
