using Godot;
using tmfos.command;
using tmfos.command.stage;

namespace tmfos.item;

/// <summary>
/// 宝箱
/// </summary>
public partial class TreasureBox : ImmediateItemPanel
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
}
