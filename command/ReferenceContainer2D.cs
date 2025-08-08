using Godot;

namespace tmfos.command;

/// <summary>
/// 他コマンドコンテナを呼び出すコマンド2D版
/// </summary>
public partial class ReferenceContainer2D : CommandNode
{
    /// <summary>
    /// 呼び出すコマンドコンテナ
    /// </summary>
    [Export]
    public CommandNode2D Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.ExecCommand(node, flag);
    }
}
