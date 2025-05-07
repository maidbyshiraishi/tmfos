using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// 画面オプションを保存するコマンド
/// </summary>
public partial class SaveScreenOptionsCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameScreenOption>("/root/GameScreenOption").SaveScreenOptions();
    }
}
