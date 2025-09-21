using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ゲームを終了するコマンド
/// </summary>
public partial class QuitGameCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").QuitGame();
    }
}
