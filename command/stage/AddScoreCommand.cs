using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.stage;

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
