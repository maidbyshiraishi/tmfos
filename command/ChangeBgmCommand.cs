using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// BGMを変更するコマンド
/// </summary>
public partial class ChangeBgmCommand : CommandNode
{
    /// <summary>
    /// MusicPlayerコマンド
    /// </summary>
    [Export]
    public MusicPlayer.Command Command { get; set; }

    /// <summary>
    /// 対象とするオーディオストリーム
    /// </summary>
    [Export]
    public AudioStream Stream { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        GetNode<MusicPlayer>("/root/MusicPlayer").Play(Command, Stream);
    }
}
