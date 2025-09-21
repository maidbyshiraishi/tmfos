using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// 操作設定をデフォルト値にリセットするコマンド
/// </summary>
public partial class ResetDefaultKeyOptionsCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GameKeyOption.ResetDefaultKeyOptions();
    }
}
