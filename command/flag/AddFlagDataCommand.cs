using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.flag;

/// <summary>
/// フラグを操作するコマンド
/// </summary>
public partial class AddFlagDataCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Key { get; set; }

    /// <summary>
    /// フラグ値
    /// </summary>
    [Export]
    public int Value { get; set; }

    public override void DoCommand(Node node, bool flag) => GetNode<GameDataManager>("/root/GameDataManager").AddFlagData(Key, Value);
}
