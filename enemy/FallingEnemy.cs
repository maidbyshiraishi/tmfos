using Godot;
using tmfos.mob;
using tmfos.player;

namespace tmfos.enemy;

/// <summary>
/// 落下する敵の親
/// </summary>
public partial class FallingEnemy : Shot
{
    [Export]
    public string FlyingSe { get; set; }

    private RayCast2D _longDown;
    private bool _falling = false;

    public override void _Ready()
    {
        base._Ready();
        _longDown = GetNode<RayCast2D>("LongDown");
    }

    public override void InitializeNode()
    {
        PlaySprite();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_longDown.IsColliding())
        {
            _falling = true;
        }

        if (_falling)
        {
            MoveLocalY((float)(Speed * delta));
        }
    }

    public override void HitArea2D(Area2D node)
    {
    }

    public override void HitNode2D(Node2D node)
    {
        if (node is Player player)
        {
            player.AddDurability(-Attack - m_attackCorrection);
        }

        if (!Penetration)
        {
            RemoveNode();
        }
    }

    public override void CalcDirection()
    {
    }
}
