using Godot;

namespace maid_by_shiraishi.command.animation;

/// <summary>
/// AnimatedSprite2Dのアニメーションを開始するコマンド
/// </summary>
public partial class PlayAnimationCommand : CommandRoot
{
    /// <summary>
    /// 開始するAnimatedSprite2D
    /// </summary>
    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set; }

    public override void DoCommand(Node node, bool flag) => AnimatedSprite?.Play();
}
