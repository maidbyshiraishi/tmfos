using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// 表示でコマンドを実行するトリガー
/// </summary>
public partial class VisibleTrigger : VisibleOnScreenNotifier2D
{
    public override void _Ready()
    {
        _ = Connect(VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new(this, MethodName.Entered));
        _ = Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new(this, MethodName.Exited));
    }

    public void Entered()
    {
        Lib.ExecCommands(this, null, true);
    }

    public void Exited()
    {
        Lib.ExecCommands(this, null, false);
    }
}
