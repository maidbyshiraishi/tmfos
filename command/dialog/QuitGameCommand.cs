using Godot;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ゲームを終了するコマンド
/// </summary>
public partial class QuitGameCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<DialogLayer>("/root/DialogLayer").QuitGame();
    }
}
