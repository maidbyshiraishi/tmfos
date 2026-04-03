using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.flag;

/// <summary>
/// フラグデータをセットするコマンド
/// </summary>
public partial class SetFlagDataCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Target { get; set; }

    /// <summary>
    /// フラグ値
    /// </summary>
    [Export]
    public int Value { get; set; }

    public override void DoCommand(Node node, bool flag) => GetNode<GameDataManager>("/root/GameDataManager").SetFlagData(Target, Value);
}
