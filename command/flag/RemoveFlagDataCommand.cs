using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.flag;

/// <summary>
/// ゲームフラグを削除するコマンド
/// </summary>
public partial class RemoveFlagDataCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Target { get; set; }

    public override void DoCommand(Node node, bool flag) => GetNode<GameDataManager>("/root/GameDataManager").RemoveFlagData(Target);
}
