using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// タイマートリガー
/// </summary>
public partial class TimerTrigger : Node
{
    /// <summary>
    /// 待機時間
    /// </summary>
    [Export]
    public double WaitTime { get; set; } = 5d;

    /// <summary>
    /// 使用するタイマー
    /// </summary>
    [Export]
    public Timer Timer { get; set; }

    public override void _Ready()
    {
        base._Ready();
        _ = Timer?.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.Exec));
        _ = Connect(Node.SignalName.TreeExiting, new Callable(this, MethodName.StopTimer));
    }

    public virtual void Exec()
    {
        Lib.ExecCommands(this, null, true);
    }

    /// <summary>
    /// タイマーを再スタートする
    /// </summary>
    public void ResetTimer()
    {
        Lib.ResetTimer(Timer, WaitTime);
    }

    /// <summary>
    /// タイマーを一時停止する
    /// </summary>
    /// <param name="paused">停止するか</param>
    public void PauseTimer(bool paused)
    {
        if (Timer is not null)
        {
            Timer.Paused = paused;
        }
    }

    public void StopTimer()
    {
        PauseTimer(true);
    }
}
