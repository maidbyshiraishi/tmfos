using Godot;
using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// ウンモ成人(テレポートする)敵
/// </summary>
public partial class UnmoEnemy : EyesightEnemy
{
    private bool _lastOnFloor;
    private bool _skipTeleport;

    public override void InitializeNode()
    {
        base.InitializeNode();
        Direction = Lib.GetLRDirection(Position, m_player.Position);
        ChangeSprite("walk", Direction);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (MobState is MobStateType.Sleep)
        {
            return;
        }

        Vector2 velocity = Velocity;

        if (MobState is MobStateType.Dead or MobStateType.Timeup || _skipTeleport)
        {
            velocity.X = 0f;
            velocity.Y += m_gravity * (float)delta;
        }
        else
        {
            // ハシゴモードと通常モードを分ける
            if (Action is not MobActionType.Climb and not MobActionType.Crouch)
            {
                velocity = NormalAction(delta, velocity);
            }
        }

        Velocity = velocity;
        _ = MoveAndSlide();

        if (DirectAttack("res://decoration/slash.tscn") && MobState is MobStateType.Dead)
        {
            SetCollisionMaskValue(4, false);
        }
    }

    private Vector2 NormalAction(double delta, Vector2 velocity)
    {
        bool onFloor = IsOnFloor();
        ChangeSprite("walk", Direction);

        // 重力の処理は常に行う
        velocity.Y += m_gravity * (float)delta;

        if (!_skipTeleport && Position.DistanceTo(m_player.Position) < 200f)
        {
            Teleport(0.8f);
        }

        if (onFloor && m_eyesight.IsFaceToWall(Direction))
        {
            // 方向転換
            Reverse();
        }
        else if (onFloor && m_eyesight.IsBrow(Direction))
        {
            // 方向転換
            Reverse();
        }

        velocity = Walk(delta, velocity, onFloor);

        if (onFloor && !_lastOnFloor)
        {
            PlaySe(TouchdownSe);
        }

        _lastOnFloor = onFloor;
        return velocity;
    }

    protected async void Teleport(double waitTime)
    {
        if (_skipTeleport)
        {
            return;
        }

        Vector2 vec = m_player.Position;
        PlaySe("unmo_charge");

        if (waitTime >= 0.05f)
        {
            _skipTeleport = true;
            _ = await ToSignal(GetTree().CreateTimer(waitTime), Timer.SignalName.Timeout);
        }

        PlaySe("unmo_teleport");
        Position = vec;
        _skipTeleport = false;
    }

    public override void Burialed(Node2D body)
    {
    }
}
