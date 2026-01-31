using Godot;
using tmfos.system;

namespace tmfos.command.option;

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
