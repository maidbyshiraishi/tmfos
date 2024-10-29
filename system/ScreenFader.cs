using Godot;

namespace tmfos.system;

/// <summary>
/// 画面遷移エフェクト制御
/// </summary>
public partial class ScreenFader : CanvasLayer
{
    public const string None = null;
    public static readonly string Fadein1 = "screen_fadein1";
    public static readonly string Fadeout1 = "screen_fadeout1";
    public static readonly string Fadein2 = "screen_fadein2";
    public static readonly string Fadeout2 = "screen_fadeout2";
    public static readonly string Fadein3 = "screen_fadein3";
    public static readonly string Fadeout3 = "screen_fadeout3";

    public AnimatedSprite2D Fader { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Fader = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
}
