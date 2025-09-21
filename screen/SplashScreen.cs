using Godot;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// スプラッシュ画面
/// </summary>
public partial class SplashScreen : DialogRoot
{
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
