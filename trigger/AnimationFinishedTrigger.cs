using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// アニメーション終了トリガー
/// </summary>
public partial class AnimationFinishedTrigger : Node
{
    /// <summary>
    /// アニメーションを終了させるAnimatedSprite2D
    /// </summary>
    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set; }

    public override void _Ready()
    {
        base._Ready();
        _ = AnimatedSprite?.Connect(AnimatedSprite2D.SignalName.AnimationFinished, new Callable(this, MethodName.Exec));
    }

    public virtual void Exec()
    {
        Lib.ExecCommands(this, null, true);
    }
}
