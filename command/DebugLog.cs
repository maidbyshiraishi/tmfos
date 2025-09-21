using Godot;

namespace tmfos.command;

/// <summary>
/// デバッグログ
/// </summary>
public partial class DebugLog : CommandNode
{
    /// <summary>
    /// ログ
    /// </summary>
    [Export]
    public string Log { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (!string.IsNullOrWhiteSpace(Log))
        {
            GD.Print(Log);
        }
    }
}
