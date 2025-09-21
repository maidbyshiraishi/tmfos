using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// BGMを一時停止するコマンド
/// </summary>
public partial class PauseBgmCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<MusicPlayer>("/root/MusicPlayer").Pause(true);
    }
}
