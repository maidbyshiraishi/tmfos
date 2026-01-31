using Godot;

namespace tmfos.command;

/// <summary>
/// 他コマンドコンテナを呼び出すコマンド
/// </summary>
public partial class ReferenceContainer : CommandRoot
{
    /// <summary>
    /// 呼び出すコマンドコンテナ
    /// </summary>
    [Export]
    public CommandRoot Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.ExecCommand(node, flag);
    }
}
