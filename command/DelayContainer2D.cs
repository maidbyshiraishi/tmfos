using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// コマンド延滞実行コンテナ2D版
/// </summary>
public partial class DelayContainer2D : CommandNode2D
{
    /// <summary>
    /// 延滞時間
    /// </summary>
    [Export]
    public double WaitTime { get; set; } = 0f;

    public override void DoCommand(Node node, bool flag)
    {
        WaitExec(node, flag);
    }

    private async void WaitExec(Node node, bool flag)
    {
        if (WaitTime >= 0.05f)
        {
            _ = await ToSignal(GetTree().CreateTimer(WaitTime), Timer.SignalName.Timeout);
        }

        Lib.ExecCommands(this, node, flag);
    }
}
