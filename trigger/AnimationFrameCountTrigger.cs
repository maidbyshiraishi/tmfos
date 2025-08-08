using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// アニメーションの指定フレームでコマンドを実行するトリガー
/// </summary>
public partial class AnimationFrameCountTrigger : Node
{
    /// <summary>
    /// コマンドを実行するフレーム
    /// </summary>
    [Export]
    public int FrameCount { get; set; }

    private int _now = 0;

    public override void _Ready()
    {
        if (GetParent() is AnimatedSprite2D animatedSprite)
        {
            _ = animatedSprite.Connect(AnimatedSprite2D.SignalName.FrameChanged, new(this, MethodName.CountUp));
        }
    }

    private void CountUp()
    {
        _now++;

        if (_now == FrameCount)
        {
            Lib.ExecCommands(this, null, true);
        }
    }
}
