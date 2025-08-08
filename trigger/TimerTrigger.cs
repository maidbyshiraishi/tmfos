using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// タイマートリガー
/// </summary>
public partial class TimerTrigger : Node
{
    private Timer _timer;

    public override void _Ready()
    {
        if (GetParent() is Timer timer)
        {
            _timer = timer;
            _ = _timer.Connect(Timer.SignalName.Timeout, new(this, MethodName.Exec));
            _ = Connect(Node.SignalName.TreeExiting, new(this, MethodName.StopTimer));
        }
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
        Lib.ResetTimer(_timer);
    }

    /// <summary>
    /// タイマーを一時停止する
    /// </summary>
    /// <param name="paused">停止するか</param>
    public void PauseTimer(bool paused)
    {
        if (_timer is not null)
        {
            _timer.Paused = paused;
        }
    }

    public void StopTimer()
    {
        PauseTimer(true);
    }
}
