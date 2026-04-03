using Godot;

namespace maid_by_shiraishi.mob;

/// <summary>
/// ブロック埋没判定
/// </summary>
public partial class BurialArea : Area2D
{
    private bool _flag = false;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        GetNode<Timer>("Timer").Timeout += Switch;

        if (GetParent() is ActionMob amob)
        {
            BodyEntered += amob.Burialed;
        }
    }

    public void Switch()
    {
        _flag = !_flag;
        _collisionShape.Disabled = _flag;
    }
}
