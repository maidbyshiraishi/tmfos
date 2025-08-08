using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// キーの開放でコマンドを実行するトリガー
/// </summary>
public partial class KeyReleaseedTrigger : Node
{
    /// <summary>
    /// コマンドを実行するアクション名
    /// </summary>
    [Export]
    public string ActionName { get; set; }

    public override void _Ready()
    {
        AddToGroup(StageRoot.PhysicsProcessGroup);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!string.IsNullOrEmpty(ActionName) && Input.IsActionJustReleased(ActionName))
        {
            Exec(this);
        }
    }

    public void Exec(Node node)
    {
        Lib.ExecCommands(this, node, true);
    }
}
