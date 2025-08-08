using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// PhysicsProcessトリガーの親
/// </summary>
public partial class PhysicsProcessTrigger : Node
{
    public override void _Ready()
    {
        AddToGroup(StageRoot.PhysicsProcessGroup);
    }

    public override void _PhysicsProcess(double delta)
    {
        Exec();
    }

    public void Exec()
    {
        Lib.ExecCommands(this, null, true);
    }
}
