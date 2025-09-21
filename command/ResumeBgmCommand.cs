using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// BGMの一時停止を解除するコマンド
/// </summary>
public partial class ResumeBgmCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<MusicPlayer>("/root/MusicPlayer").Pause(false);
    }
}
