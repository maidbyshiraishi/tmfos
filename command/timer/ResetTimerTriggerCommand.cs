using Godot;
using tmfos.trigger;

namespace tmfos.command.timer;

/// <summary>
/// タイマートリガーをリセットして再開するコマンド
/// </summary>
public partial class ResetTimerTriggerCommand : CommandNode
{
    /// <summary>
    /// リセットするタイマートリガー
    /// </summary>
    [Export]
    public TimerTrigger Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.ResetTimer();
    }
}
