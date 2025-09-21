using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// コマンドコンテナ
/// </summary>
public partial class CommandContainer : CommandNode
{
    public override void ExecCommand(Node node, bool flag)
    {
        DoCommand(node, flag);
    }

    public override void DoCommand(Node node, bool flag)
    {
        Lib.ExecCommands(this, node, flag);
    }
}
