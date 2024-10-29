using Godot;
using tmfos.trigger;

namespace tmfos.command;

/// <summary>
/// トリガーの一回のみ実行をリセットするコマンド2D版
/// </summary>
public partial class ResetOneTimeCommand2D : CommandNode
{
    /// <summary>
    /// リセットするトリガー
    /// </summary>
    [Export]
    public Trigger2D Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        Target?.SetOpened(false);
    }
}
