using Godot;
using tmfos.system;

namespace tmfos.command.option;

/// <summary>
/// 操作設定を保存するコマンド
/// </summary>
public partial class SaveKeyOptionsCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameKeyOption>("/root/GameKeyOption").SaveKeyOptions();
    }
}
