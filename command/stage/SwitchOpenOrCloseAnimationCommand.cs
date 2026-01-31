using Godot;

namespace tmfos.command.stage;

/// <summary>
/// 開閉アニメーションを切り替える
/// </summary>
public partial class SwitchOpenOrCloseAnimationCommand : CommandRoot
{
    private bool _opened;

    /// <summary>
    /// 開閉アニメーションを切り替えるAnimatedSprite2D
    /// </summary>
    [Export]
    public AnimatedSprite2D AnimatedSprite2D { get; set; }

    /// <summary>
    /// 開閉状態
    /// </summary>
    [Export]
    public bool Opened
    {
        get => _opened;

        set
        {
            _opened = value;
            AnimatedSprite2D?.Play(Opened ? "opened" : "closed");
        }
    }

    public override void DoCommand(Node node, bool flag)
    {
        Opened = !_opened;
    }
}
