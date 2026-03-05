using Godot;

namespace tmfos.system;

/// <summary>
/// 画面遷移エフェクト制御
/// </summary>
public partial class ScreenFader : CanvasLayer
{
    [Signal]
    public delegate void ScreenFadeFinishedEventHandler();

    private AnimatedSprite2D _animatedSprite2D;

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite2D.AnimationFinished += AnimationFinished;
    }

    public void ScreenFade(string effectName)
    {
        if (!string.IsNullOrWhiteSpace(effectName) && _animatedSprite2D.SpriteFrames.HasAnimation(effectName))
        {
            StartFader(_animatedSprite2D, effectName);
            return;
        }

        WaitProcessFrame();
    }

    private async void StartFader(AnimatedSprite2D fader, string effectName)
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        fader.Play(effectName);
    }

    private async void WaitProcessFrame()
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        AnimationFinished();
    }

    public void AnimationFinished() => EmitSignal(SignalName.ScreenFadeFinished);
}
