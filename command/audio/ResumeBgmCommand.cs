using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.audio;

/// <summary>
/// BGMの一時停止を解除するコマンド
/// </summary>
public partial class ResumeBgmCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<MusicPlayer>("/root/MusicPlayer").Pause(false);
}
