using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// アニメーションの指定フレームでコマンドを実行するトリガー
/// </summary>
public partial class AnimationFrameCountTrigger : Node
{
    /// <summary>
    /// 再生中のAnimatedSprite2D
    /// </summary>
    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set; }

    /// <summary>
    /// コマンドを実行するフレーム
    /// </summary>
    [Export]
    public int FrameCount { get; set; }

    private int _now = 0;

    public override void _Ready()
    {
        base._Ready();
        _ = AnimatedSprite?.Connect(AnimatedSprite2D.SignalName.FrameChanged, new Callable(this, MethodName.CountUp));
    }

    private void CountUp()
    {
        if (AnimatedSprite is null)
        {
            return;
        }

        _now++;

        if (_now == FrameCount)
        {
            Lib.ExecCommands(this, null, true);
        }
    }
}
