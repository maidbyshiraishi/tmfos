using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

/// <summary>
/// ゲームオプションをセットするコマンド
/// </summary>
public partial class SetGameOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.SetOptions();
    }
}
