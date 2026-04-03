using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// PhysicsProcessトリガーの親
/// </summary>
public partial class PhysicsProcessTrigger : Node
{
    public override void _Ready() => AddToGroup(GameStageRoot.PhysicsProcessGroup);

    public override void _PhysicsProcess(double delta) => Exec();

    public void Exec() => Lib.ExecCommands(this, null, true);
}
