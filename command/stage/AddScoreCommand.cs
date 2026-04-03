using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// 得点を操作するコマンド
/// </summary>
public partial class AddScoreCommand : CommandRoot
{
    /// <summary>
    /// 得点
    /// </summary>
    [Export]
    public int Score { get; set; } = 3000;

    public override void DoCommand(Node node, bool flag)
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        stageRoot.AddScore(Score);
    }
}
