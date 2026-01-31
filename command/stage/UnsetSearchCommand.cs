using Godot;
using tmfos.mob;

namespace tmfos.command.stage;

/// <summary>
/// 探索状態を解除するコマンド
/// </summary>
public partial class UnsetSearchCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        if (node is ActionMob amob)
        {
            amob.Search = false;
        }
    }
}
