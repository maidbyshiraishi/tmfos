using Godot;
using Godot.Collections;
using System.Text.RegularExpressions;
using tmfos.command;
using tmfos.decoration;
using tmfos.mob;
using tmfos.stage;

namespace tmfos.system;

/// <summary>
/// 便利関数
/// </summary>
public static partial class Lib
{
    public static string GenerateName(Node node)
    {
        return MyRegex().Replace(node.GetPath().ToString(), "");
    }

    public static string JoinString(Array array)
    {
        string text = "";

        foreach (Variant var in array)
        {
            if (var.VariantType is Variant.Type.String)
            {
                text += var.AsString() + "\n";
            }
        }

        return text.TrimEnd('\n');
    }

    public static T GetPackedScene<T>(string path) where T : class
    {
        // todo: 利用箇所がクドいのでスッキリ書きたい
        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがnullまたはホワイトスペースです。リソースファイルを読み込めません。");
            return null;
        }

        if (ResourceLoader.Load(path) is T pack)
        {
            return pack;
        }

        GD.PrintErr($"リソースファイル{path}が存在しません。");
        return null;
    }

    public static void ShowFloatingMessage(Node node, string text, Color color)
    {
        if (GetPackedScene<PackedScene>("res://decoration/floating_message.tscn") is PackedScene pack && pack.Instantiate() is FloatingMessage fmsg)
        {
            fmsg.Text = text;
            fmsg.Color = color;
            node.AddChild(fmsg);
        }
    }

    public static DirectionType GetUDLRDirection(Vector2 v1, Vector2 v2)
    {
        if (Mathf.Abs(v1.X - v2.X) >= Mathf.Abs(v1.Y - v2.Y))
        {
            //a
            return GetLRDirection(v1, v2);
        }

        return GetUDDirection(v1, v2);
    }

    public static DirectionType GetUDDirection(Vector2 v1, Vector2 v2)
    {
        if (v1.Y >= v2.Y)
        {
            //a
            return DirectionType.Down;
        }

        return DirectionType.Up;
    }

    public static DirectionType GetLRDirection(Vector2 v1, Vector2 v2)
    {
        if (v1.X >= v2.X)
        {
            //a
            return DirectionType.Left;
        }

        return DirectionType.Right;
    }

    public static void ExecCommands(Node root, Node node, bool flag)
    {
        Array<Node> children = root.GetChildren();

        foreach (Node n in children)
        {
            if (n is ICommand cnode && n.Name != "Focus" && n.Name != "Toggled")
            {
                cnode.ExecCommand(node, flag);
            }
        }
    }

    public static void Exec(Node root, Node node, bool flag)
    {
        if (root is not Control control || control.FocusMode == Control.FocusModeEnum.None)
        {
            return;
        }

        CommandContainer target = root.GetNodeOrNull<CommandContainer>("Exec");
        target?.ExecCommand(node, flag);
    }

    public static void Focus(Node root, Node node, bool flag)
    {
        if (root is not Control control || control.FocusMode == Control.FocusModeEnum.None)
        {
            return;
        }

        CommandContainer target = root.GetNodeOrNull<CommandContainer>("Focus");
        target?.ExecCommand(node, flag);
    }

    public static void Toggled(Node root, Node node, bool flag)
    {
        if (root is not Control control || control.FocusMode == Control.FocusModeEnum.None)
        {
            return;
        }

        CommandContainer target = root.GetNodeOrNull<CommandContainer>("Toggled");
        target?.ExecCommand(node, flag);
    }

    public static void SetPhysics(Node root, bool enabled)
    {
        root.GetTree().CallGroup(StageRoot.PhysicsProcessGroup, Node.MethodName.SetPhysicsProcess, enabled);
    }

    public static void GrabFocus(Control control)
    {
        control.SetBlockSignals(true);
        control.GrabFocus();
        control.SetBlockSignals(false);
    }

    public static void ConnectFocusSignal(Control control, Callable entered, Callable exited, Callable moudeEntered)
    {
        _ = control.Connect(Control.SignalName.FocusEntered, entered);
        _ = control.Connect(Control.SignalName.MouseEntered, moudeEntered);
        _ = control.Connect(Control.SignalName.FocusExited, exited);
        _ = control.Connect(Control.SignalName.MouseExited, exited);
    }

    public static void ResetTimer(Timer timer)
    {
        if (timer is null || Mathf.Abs(timer.WaitTime) < 0.05f)
        {
            return;
        }

        timer.Paused = false;
        _ = timer.CallDeferred(Timer.MethodName.Start);
    }

    public static void InitializeNodeAll(Node node)
    {
        if (node is IGameNode inode)
        {
            inode.InitializeNode();
        }

        Array<Node> list = node.GetChildren();

        foreach (Node item in list)
        {
            InitializeNodeAll(item);
        }
    }

    public static void UpdateLabel(Label label, string text)
    {
        if (label is null)
        {
            return;
        }

        if (!label.Visible)
        {
            label.Show();
        }

        label.Text = text;
    }

    [GeneratedRegex("^/root/.*?/")]
    private static partial Regex MyRegex();
}
