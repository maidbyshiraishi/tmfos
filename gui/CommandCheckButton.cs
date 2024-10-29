using Godot;
using tmfos.system;

namespace tmfos.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行するチェックボタン
/// </summary>
public partial class CommandCheckButton : CheckButton
{
    public override void _Ready()
    {
        base._Ready();
        _ = Connect(BaseButton.SignalName.Toggled, new Callable(this, MethodName.ExecToggled));
        Lib.ConnectFocusSignal(this, new Callable(this, MethodName.ExecFocusEntered), new Callable(this, MethodName.ExecFocusExited));
    }

    public virtual void ExecToggled(bool toggledOn)
    {
        Lib.Toggled(this, null, toggledOn);
    }

    public virtual void ExecFocusEntered()
    {
        Lib.Focus(this, null, true);
    }

    public virtual void ExecFocusExited()
    {
        Lib.Focus(this, null, false);
    }
}
