using Godot;
using tmfos.system;

namespace tmfos.command.audio;

/// <summary>
/// 効果音を再生するコマンド
/// </summary>
public partial class PlaySeCommand : CommandNode
{
    /// <summary>
    /// 効果音名
    /// </summary>
    [Export]
    public string SeName { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<SePlayer>("/root/SePlayer").Play(SeName);
    }
}
