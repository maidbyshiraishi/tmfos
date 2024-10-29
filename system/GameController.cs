using Godot;

namespace tmfos.system;

/// <summary>
/// ゲームの入力の連打、押しっぱなしを検出する
/// </summary>
public partial class GameController : Node
{
    public enum ButtonState
    {
        /// <summary>
        /// 解放
        /// </summary>
        Release = 0,

        /// <summary>
        /// 押下
        /// </summary>
        Press,

        /// <summary>
        /// 連打
        /// </summary>
        Repeat,

        /// <summary>
        /// なし
        /// </summary>
        Null
    }

    private bool _running = false;
    private Timer _timer;
    private ButtonState _a = ButtonState.Release;
    private ButtonState _b = ButtonState.Release;
    private bool _local;

    public override void _Ready()
    {
        base._Ready();
        _timer = GetNode<Timer>("Timer");
    }

    public override void _Process(double delta)
    {
        UpdateStatus(Input.IsActionPressed("ui_accept"), ref _a);
        UpdateStatus(Input.IsActionPressed("b"), ref _b);
    }

    public ButtonState GetState(string action)
    {
        return action switch
        {
            "ui_accept" => _a,
            "b" => _b,
            _ => ButtonState.Null,
        };
    }

    public void TimerTimeout()
    {
        _running = false;
    }

    protected void TimerStart()
    {
        _ = _timer.CallDeferred(Timer.MethodName.Start);
        _running = true;
    }

    private void UpdateStatus(bool raw, ref ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Release:

                if (raw)
                {
                    state = ButtonState.Press;
                    _local = true;
                    TimerStart();
                }

                break;

            case ButtonState.Press:

                if (_local)
                {
                    if (!raw)
                    {
                        _local = false;
                        TimerStart();
                    }
                }
                else
                {
                    if (_running && raw)
                    {
                        state = ButtonState.Repeat;
                        _local = true;
                        TimerStart();
                    }
                    else if (_running)
                    {
                        // 状態維持
                    }
                    else
                    {
                        state = ButtonState.Release;
                    }
                }

                break;

            case ButtonState.Repeat:

                if (_local)
                {
                    if (raw)
                    {
                        if (!_running)
                        {
                            state = ButtonState.Press;
                            _local = true;
                            TimerStart();
                        }
                    }
                    else
                    {
                        _local = false;
                        TimerStart();
                    }
                }
                else
                {
                    if (raw)
                    {
                        _local = true;
                        TimerStart();
                    }
                    else
                    {
                        if (!_running)
                        {
                            state = ButtonState.Release;
                        }
                    }
                }

                break;
        }
    }
}
