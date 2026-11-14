using Godot;

namespace maid_by_shiraishi.system;

/// <summary>
/// 画面遷移エフェクト制御
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// アニメーションの初期値としてnoneを指定しなければ、スプラッシュスクリーンが表示されない場合がある。
/// </summary>
public partial class ScreenFader : CanvasLayer
{
    [Signal]
    public delegate void ScreenFadeFinishedEventHandler();

    private AnimatedSprite2D _animatedSprite2D;

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite2D.Animation = "none";
        _animatedSprite2D.AnimationFinished += AnimationFinished;
    }

    public void ScreenFade(string effectName)
    {
        if (!string.IsNullOrWhiteSpace(effectName) && _animatedSprite2D.SpriteFrames.HasAnimation(effectName))
        {
            _animatedSprite2D.Play(effectName);
            return;
        }

        AnimationFinished();
    }

    public async void AnimationFinished()
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = EmitSignal(SignalName.ScreenFadeFinished);
    }
}
