using Godot;
using tmfos.mob;
using tmfos.player;

namespace tmfos.door;

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
        _ = Connect(Area2D.SignalName.AreaExited, new(this, MethodName.EnableWarp));
    }
}
