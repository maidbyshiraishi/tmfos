using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

/// <summary>
/// ゲーム設定をロードするコマンド
/// </summary>
public partial class LoadGameOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.LoadOptions();
    }
}
