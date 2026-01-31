using Godot;
using tmfos.system;

namespace tmfos.command.option;

/// <summary>
/// 操作設定をロードするコマンド
/// </summary>
public partial class LoadKeyOptionsCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameKeyOption>("/root/GameKeyOption").LoadKeyOptions();
    }
}
