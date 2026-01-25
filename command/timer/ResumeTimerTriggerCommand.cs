using Godot;
using tmfos.trigger;

namespace tmfos.command.timer;

/// <summary>
/// タイマートリガーの一時停止を解除するコマンド
/// </summary>
public partial class ResumeTimerTriggerCommand : CommandNode
{
    /// <summary>
    /// 一時停止を解除するタイマートリガー
    /// </summary>
    [Export]
    public TimerTrigger Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.PauseTimer(false);
    }
}
