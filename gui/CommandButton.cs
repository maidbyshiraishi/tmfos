using Godot;
using tmfos.system;

namespace tmfos.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行するボタン
/// </summary>
public partial class CommandButton : Button
{
    // todo: ボタンなどの各種シグナルを自動的に接続したい。

    public override void _Ready()
    {
        base._Ready();
        _ = Connect(BaseButton.SignalName.Pressed, new Callable(this, MethodName.Exec));
        Lib.ConnectFocusSignal(this, new Callable(this, MethodName.ExecFocusEntered), new Callable(this, MethodName.ExecFocusExited));
    }

    public virtual void Exec()
    {
        Lib.Exec(this, null, true);
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
