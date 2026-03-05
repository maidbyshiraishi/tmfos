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
        GetNode<AudioStreamPlayer>("Audio_1").Finished += PlayAudio2;
        GetNode<AudioStreamPlayer>("Audio_2").Finished += GoNextScreen;
        GetNode<VisibleOnScreenNotifier2D>("PlayStart").ScreenEntered += PlayStart;
    }

    /// <summary>
    /// ロゴアニメーションと効果音の再生を開始する
    /// </summary>
    public void PlayStart() => GetNode<AudioStreamPlayer>("Audio_1").Play();

    public void PlayAudio2()
    {
        GetNode<AnimatedSprite2D>("Mask/Image").Frame = 1;
        GetNode<AudioStreamPlayer>("Audio_2").Play();
    }

    /// <summary>
    /// 次画面に遷移する
    /// </summary>
    public void GoNextScreen() => GetNode<DialogLayer>("/root/DialogLayer").OpenScreen("res://screen/title_screen.tscn", "fadeout_1", "fadein_1");
}
