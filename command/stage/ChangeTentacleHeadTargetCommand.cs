using Godot;
using tmfos.enemy;

namespace tmfos.command.stage;

/// <summary>
/// TentacleHeadのターゲットを変更するコマンド
/// </summary>
public partial class ChangeTentacleHeadTargetCommand : CommandRoot
{
    [Export]
    public TentacleHead Head { get; set; }

    /// <summary>
    /// 変更先ターゲット
    /// </summary>
    [Export]
    public Node2D Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (Head is not null)
        {
            Head.Target = Target;
        }
    }
}
