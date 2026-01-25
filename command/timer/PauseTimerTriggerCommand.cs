using Godot;
using tmfos.trigger;

namespace tmfos.command.timer;

/// <summary>
/// タイマートリガーを一時停止するコマンド
/// </summary>
public partial class PauseTimerTriggerCommand : CommandNode
{
    /// <summary>
    /// 一時停止するタイマートリガー
    /// </summary>
    [Export]
    public TimerTrigger Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.PauseTimer(true);
    }
}
