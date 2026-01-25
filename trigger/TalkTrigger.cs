using Godot;
using Godot.Collections;
using tmfos.command.dialog;

namespace tmfos.trigger;

/// <summary>
/// 会話イベントトリガー
/// </summary>
public partial class TalkTrigger : TriggerArea2D
{
    /// <summary>
    /// ネコミミメイドの色指定
    /// ネコミミメイドではないならNone
    /// </summary>
    [Export]
    public ColorType Color { get; set; } = ColorType.None;

    /// <summary>
    /// 開くダイアログ
    /// </summary>
    [Export]
    public string DialogPath { get; set; }

    /// <summary>
    /// ネコミミメイドの色
    /// </summary>
    public enum ColorType
    {
        None = 0,
        Blue,
        Gold,
        Pink
    }

    private Dictionary resources = new() {
        {(int)ColorType.None, ""},
        {(int)ColorType.Blue, "res://contents/animation/theater/talk_nekomimi_maid_blue.tres"},
        {(int)ColorType.Gold, "res://contents/animation/theater/talk_nekomimi_maid_gold.tres"},
        {(int)ColorType.Pink, "res://contents/animation/theater/talk_nekomimi_maid_pink.tres"}
    };

    public override void _Ready()
    {
        base._Ready();
        OpenDialogCommand openDialogCommand = GetNode<OpenDialogCommand>("OpenDialogCommand");
        openDialogCommand.DialogPath = DialogPath;

        if (Color is not ColorType.None)
        {
            AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            animatedSprite2D.SpriteFrames = GD.Load<SpriteFrames>(resources[(int)Color].AsString());
            animatedSprite2D.Play();
        }
    }
}
