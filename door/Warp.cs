using Godot;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.player;

namespace maid_by_shiraishi.door;

/// <summary>
/// ステージ間移動の出入り口
/// </summary>
public partial class Warp : Gateway
{
    public void EnableWarp(Area2D node)
    {
        if (node is EventFinder finder && finder.EventNode2D is Player)
        {
            Disable = false;
        }
    }

    internal void DisableWarp()
    {
        Disable = true;
        AreaExited += EnableWarp;
    }
}
