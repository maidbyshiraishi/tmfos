using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ゲームオプションを保存するコマンド
/// </summary>
public partial class SaveGameOptionsCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameOption>("/root/GameOption").SaveOptions();
    }
}
