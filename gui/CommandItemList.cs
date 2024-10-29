using Godot;
using tmfos.system;

namespace tmfos.gui;

/// <summary>
/// フォーカスの入出時にコマンドを実行するアイテムリスト
/// </summary>
public partial class CommandItemList : ItemList
{
    public override void _Ready()
    {
        base._Ready();
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
