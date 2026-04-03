using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行するチェックボタン
/// </summary>
public partial class CommandCheckButton : CheckButton
{
    public override void _Ready()
    {
        Toggled += ExecToggled;
        Lib.ConnectFocusSignal(this, ExecFocusEntered, ExecFocusExited, ExecMouseEntered);
    }

    public virtual void ExecToggled(bool toggledOn) => Lib.Toggled(this, null, toggledOn);

    public virtual void ExecFocusEntered() => Lib.Focus(this, null, true);

    public virtual void ExecMouseEntered()
    {
        if (FocusMode != FocusModeEnum.None)
        {
            GrabFocus();
        }
    }

    public virtual void ExecFocusExited() => Lib.Focus(this, null, false);
}
