using Godot;
using maid_by_shiraishi.trigger;

namespace maid_by_shiraishi.command.timer;

/// <summary>
/// タイマートリガーをリセットして再開するコマンド
/// </summary>
public partial class ResetTimerTriggerCommand : CommandRoot
{
    /// <summary>
    /// リセットするタイマートリガー
    /// </summary>
    [Export]
    public TimerTrigger Target { get; set; }

    public override void DoCommand(Node node, bool flag) => Target?.ResetTimer();
}
