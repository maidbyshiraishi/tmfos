using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// 非表示でコマンドを実行するトリガー
/// </summary>
public partial class InvisibleTrigger : VisibleOnScreenNotifier2D
{
    public override void _Ready()
    {
        base._Ready();
        _ = Connect(VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new Callable(this, MethodName.Exited));
        _ = Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new Callable(this, MethodName.Entered));
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
