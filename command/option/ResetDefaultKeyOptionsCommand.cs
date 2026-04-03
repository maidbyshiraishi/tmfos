using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

/// <summary>
/// 操作設定をデフォルト値にリセットするコマンド
/// </summary>
public partial class ResetDefaultKeyOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GameKeyOption.ResetDefaultKeyOptions();
}
