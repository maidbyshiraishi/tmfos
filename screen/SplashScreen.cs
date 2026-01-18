using Godot;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// スプラッシュ画面
/// </summary>
public partial class SplashScreen : DialogRoot
{
    public override void _Ready()
    {
        base._Ready();
        _ = GetNodeOrNull<AudioStreamPlayer>("Audio_1")?.Connect(AudioStreamPlayer.SignalName.Finished, new(this, MethodName.PlayAudio2));
        _ = GetNodeOrNull<AudioStreamPlayer>("Audio_2")?.Connect(AudioStreamPlayer.SignalName.Finished, new(this, MethodName.GoNextScreen));
        _ = GetNodeOrNull<VisibleOnScreenNotifier2D>("PlayStart")?.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new(this, MethodName.PlayStart));
    }

    /// <summary>
    /// ロゴアニメーションと効果音の再生を開始する
    /// </summary>
    public void PlayStart()
    {
        GetNode<AudioStreamPlayer>("Audio1").Play();
    }

    public void PlayAudio2()
    {
        GetNode<AnimatedSprite2D>("Mask/Image").Frame = 1;
        GetNode<AudioStreamPlayer>("Audio2").Play();
    }

    /// <summary>
    /// 次画面に遷移する
    /// </summary>
    public void GoNextScreen()
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenScreen("res://screen/title_screen.tscn", "fadeout_1", "fadein_1");
    }
}
