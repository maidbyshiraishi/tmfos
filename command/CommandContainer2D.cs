using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// コマンドコンテナ2D版
/// </summary>
public partial class CommandContainer2D : CommandNode2D
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
