using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command;

/// <summary>
/// コマンドコンテナ
/// </summary>
public partial class CommandContainer : CommandRoot
{
    public override void ExecCommand(Node node, bool flag) => DoCommand(node, flag);

    public override void DoCommand(Node node, bool flag) => Lib.ExecCommands(this, node, flag);
}
