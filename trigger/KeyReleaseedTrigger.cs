using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// キーの開放でコマンドを実行するトリガー
/// </summary>
public partial class KeyReleaseedTrigger : Node
{
    public static readonly string KeyTriggerGroup = "KeyTriggerGroup";

    /// <summary>
    /// コマンドを実行するアクション名
    /// </summary>
    [Export]
    public string ActionName { get; set; }

    private bool _enabled = true;

    public override void _Ready()
    {
        AddToGroup(StageRoot.PhysicsProcessGroup);
        AddToGroup(KeyTriggerGroup);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_enabled && !string.IsNullOrWhiteSpace(ActionName) && Input.IsActionJustReleased(ActionName))
        {
            Exec(this);
            GetTree().CallGroup(KeyTriggerGroup, MethodName.WaitKey);
        }
    }

    public async void WaitKey()
    {
        // キー操作をポーズから復帰した画面やダイアログが拾ってしまうため
        // 操作後0.05秒間、キー操作を無効にする。
        _enabled = false;
        _ = await ToSignal(GetTree().CreateTimer(0.05f), Timer.SignalName.Timeout);
        _enabled = true;
    }

    public void Exec(Node node)
    {
        Lib.ExecCommands(this, node, true);
    }
}
