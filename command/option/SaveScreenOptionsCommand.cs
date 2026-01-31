using Godot;
using tmfos.system;

namespace tmfos.command.option;

/// <summary>
/// 画面オプションを保存するコマンド
/// </summary>
public partial class SaveScreenOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameScreenOption>("/root/GameScreenOption").SaveScreenOptions();
    }
}
