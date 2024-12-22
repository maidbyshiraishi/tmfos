using Godot;
using tmfos.system;

namespace tmfos.gui;

/// <summary>
/// フォーカスの入出時にコマンドを実行するテキストエリア
/// </summary>
public partial class CommandTextEdit : TextEdit
{
    public override void _Ready()
    {
        Lib.ConnectFocusSignal(this, new Callable(this, MethodName.ExecFocusEntered), new Callable(this, MethodName.ExecFocusExited));
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
