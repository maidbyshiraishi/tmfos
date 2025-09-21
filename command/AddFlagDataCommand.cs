using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// フラグを操作するコマンド
/// </summary>
public partial class AddFlagDataCommand : CommandNode
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

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameData>("/root/GameData").AddFlagData(Key, Value);
    }
}
