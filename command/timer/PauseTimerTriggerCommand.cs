using Godot;
using maid_by_shiraishi.trigger;

namespace maid_by_shiraishi.command.timer;

/// <summary>
/// タイマートリガーを一時停止するコマンド
/// </summary>
public partial class PauseTimerTriggerCommand : CommandRoot
{
    /// <summary>
    /// 一時停止するタイマートリガー
    /// </summary>
    [Export]
    public TimerTrigger Target { get; set; }

    public override void DoCommand(Node node, bool flag) => Target?.PauseTimer(true);
}
