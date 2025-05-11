using Godot;
using tmfos.system;

namespace tmfos.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行する横スライダー
/// </summary>
public partial class CommandHSlider : HSlider
{
    public override void _Ready()
    {
        Lib.ConnectFocusSignal(this, new(this, MethodName.ExecFocusEntered), new(this, MethodName.ExecFocusExited), new(this, MethodName.ExecMouseEntered));
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
