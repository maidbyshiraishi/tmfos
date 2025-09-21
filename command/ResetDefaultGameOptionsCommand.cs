using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ゲーム設定をデフォルト値にリセットするコマンド
/// </summary>
public partial class ResetDefaultGameOptionsCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.ResetOptions();
    }
}
