using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

/// <summary>
/// ゲームオプションを保存するコマンド
/// </summary>
public partial class SaveGameOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<GameOption>("/root/GameOption").SaveOptions();
}
