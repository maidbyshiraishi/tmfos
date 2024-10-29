using Godot;
using Godot.Collections;

namespace tmfos.trigger;

/// <summary>
/// グループ内のノード数がゼロになった場合にコマンドを実行するトリガー
/// </summary>
public partial class SweepObserver : PhysicsProcessTrigger
{
    /// <summary>
    /// 監視するグループ
    /// </summary>
    [Export]
    public string GroupName { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        if (string.IsNullOrWhiteSpace(GroupName))
        {
            return;
        }

        Array<Node> group = GetTree().GetNodesInGroup(GroupName);

        if (group.Count == 0)
        {
            Exec();
        }
    }
}
