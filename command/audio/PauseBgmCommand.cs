using Godot;
using tmfos.system;

namespace tmfos.command.audio;

/// <summary>
/// BGMを一時停止するコマンド
/// </summary>
public partial class PauseBgmCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<MusicPlayer>("/root/MusicPlayer").Pause(true);
    }
}
