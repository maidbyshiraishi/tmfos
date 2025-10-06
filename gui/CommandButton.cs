using Godot;
using tmfos.system;

namespace tmfos.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行するボタン
/// </summary>
public partial class CommandButton : Button
{
    // todo: ボタンなどの各種シグナルを自動的に接続したい。
    // teosにて対応済みだが逆輸入は行わない。

    public override void _Ready()
    {
        _ = Connect(BaseButton.SignalName.Pressed, new(this, MethodName.Exec));
        Lib.ConnectFocusSignal(this, new(this, MethodName.ExecFocusEntered), new(this, MethodName.ExecFocusExited), new(this, MethodName.ExecMouseEntered));
    }

    public virtual void Exec()
    {
        Lib.Exec(this, null, true);
    }

    public virtual void ExecFocusEntered()
    {
        Lib.Focus(this, null, true);
    }

    public virtual void ExecMouseEntered()
    {
        if (FocusMode != FocusModeEnum.None)
        {
            GrabFocus();
        }
    }

    public virtual void ExecFocusExited()
    {
        Lib.Focus(this, null, false);
    }
}
