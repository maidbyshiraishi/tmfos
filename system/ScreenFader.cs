using Godot;

namespace tmfos.system;

/// <summary>
/// 画面遷移エフェクト制御
/// </summary>
public partial class ScreenFader : CanvasLayer
{
    [Signal]
    public delegate void ScreenFadeFinishedEventHandler();

    public void ScreenFade(string effectName)
    {
        if (GetNodeOrNull("AnimatedSprite2D") is AnimatedSprite2D fader && !string.IsNullOrWhiteSpace(effectName) && fader.SpriteFrames.HasAnimation(effectName))
        {
            fader.Play(effectName);
            return;
        }

        WaitProcessFrame();
    }

    private async void WaitProcessFrame()
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        AnimationFinished();
    }

    public void AnimationFinished()
    {
        _ = EmitSignal(SignalName.ScreenFadeFinished);
    }
}
