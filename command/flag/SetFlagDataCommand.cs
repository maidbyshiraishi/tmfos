using Godot;
using tmfos.system;

namespace tmfos.command.flag;

/// <summary>
/// フラグデータをセットするコマンド
/// </summary>
public partial class SetFlagDataCommand : CommandNode
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

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameData>("/root/GameData").SetFlagData(Target, Value);
    }
}
