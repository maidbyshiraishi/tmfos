using Godot;
using tmfos.command;
using tmfos.trigger;

namespace tmfos.item;

/// <summary>
/// 宝箱
/// </summary>
public partial class TreasureBox : Trigger2D
{
    /// <summary>
    /// 得点
    /// </summary>
    [Export]
    public int Score { get; set; } = 10000;

    public override void _Ready()
    {
        base._Ready();
        AddScoreCommand addScoreCommand = GetNode<AddScoreCommand>("AddScoreCommand");
        addScoreCommand.Score = Score;
        ShowFloatingMessageCommand showFloatingMessageCommand = GetNode<ShowFloatingMessageCommand>("ShowFloatingMessageCommand");
        showFloatingMessageCommand.Message = Score.ToString();
    }

    public override void SetOpened(bool opened)
    {
        base.SetOpened(opened);
        SwitchOpenOrCloseAnimationCommand switchOpenOrCloseAnimationCommand = GetNode<SwitchOpenOrCloseAnimationCommand>("SwitchOpenOrCloseAnimationCommand");
        switchOpenOrCloseAnimationCommand.Opened = opened;
    }
}
