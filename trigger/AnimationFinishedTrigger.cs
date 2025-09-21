using Godot;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// アニメーション終了トリガー
/// </summary>
public partial class AnimationFinishedTrigger : Node
{
    public override void _Ready()
    {
        if (GetParent() is AnimatedSprite2D animatedSprite)
        {
            _ = animatedSprite.Connect(AnimatedSprite2D.SignalName.AnimationFinished, new(this, MethodName.Exec));
        }
    }

    public virtual void Exec()
    {
        Lib.ExecCommands(this, null, true);
    }
}
