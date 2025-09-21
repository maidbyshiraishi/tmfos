using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ゲームフラグを削除するコマンド
/// </summary>
public partial class RemoveFlagDataCommand : CommandNode
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameData>("/root/GameData").RemoveFlagData(Target);
    }
}
