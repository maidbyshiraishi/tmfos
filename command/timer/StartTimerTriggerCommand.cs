using Godot;
using tmfos.trigger;

namespace tmfos.command.timer;

/// <summary>
/// タイマートリガーを開始するコマンド
/// 一時停止も解除される
/// </summary>
public partial class StartTimerTriggerCommand : CommandRoot
{
    /// <summary>
    /// 開始するタイマートリガー
    /// </summary>
    [Export]
    public TimerTrigger Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.ResetTimer();
    }
}
