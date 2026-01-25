using Godot;
using tmfos.system;

namespace tmfos.command.option;

/// <summary>
/// ゲームオプションをセットするコマンド
/// </summary>
public partial class SetGameOptionsCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.SetOptions();
    }
}
