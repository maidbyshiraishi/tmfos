using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

/// <summary>
/// ゲーム設定をデフォルト値にリセットするコマンド
/// </summary>
public partial class ResetDefaultGameOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.ResetOptions();
    }
}
