using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// 非表示でコマンドを実行するトリガー
/// </summary>
public partial class InvisibleTrigger : VisibleOnScreenNotifier2D
{
    public override void _Ready()
    {
        ScreenEntered += Exited;
        ScreenExited += Entered;
    }

    public void Entered() => Lib.ExecCommands(this, null, true);

    public void Exited() => Lib.ExecCommands(this, null, false);
}
