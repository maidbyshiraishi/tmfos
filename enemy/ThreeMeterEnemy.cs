using Godot;
using tmfos.mob;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 3メートルの宇宙人敵
/// </summary>
public partial class ThreeMeterEnemy : EyesightEnemy, ISwimAction
{
    private bool _lastOnFloor;

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

        if (MobState is MobStateType.Dead or MobStateType.Timeup)
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
    }

    public void SetSwim()
    {
        Dead();
    }

    public void UnsetSwim()
    {
    }

    private Vector2 NormalAction(double delta, Vector2 velocity)
    {
        bool onFloor = IsOnFloor();
        ChangeSprite("walk", Direction);

        // 重力の処理は常に行う
        velocity.Y += m_gravity * (float)delta;

        // 地上にいる
        if (onFloor)
        {
            // 主人公の攻撃はジャンプで避ける
            if (m_eyesight.IsAvoidPlayerShot(Direction))
            {
                velocity.Y = m_shortJumpVelocity;
                PlaySe(JumpSe);
                // とりあえずジャンプ時に攻撃する
                Shot();
            }
            // 主人公が視界に入ったら方向転換する
            else if (!SkipAttack && m_eyesight.InSightOfPlayer())
            {
                Direction = Lib.GetLRDirection(Position, m_player.Position);
                Shot();
            }
            // 左右を壁に囲まれたら、とりあえずジャンプしてみる
            else if (m_eyesight.IsEncloseWall())
            {
                velocity.Y = m_jumpVelocity;
                PlaySe(JumpSe);
                // とりあえずジャンプ時に攻撃する
                Shot();
            }
            // 壁に接近した
            else if (m_eyesight.IsFaceToWall(Direction))
            {
                if (m_eyesight.IsJumpWall(Direction))
                {
                    // ジャンプできる場合はジャンプする
                    velocity.Y = m_jumpVelocity;
                    PlaySe(JumpSe);
                    // とりあえずジャンプ時に攻撃する
                    Shot();
                }
                else
                {
                    // 方向転換
                    Reverse();
                }
            }
            // 目の前が崖の場合は飛び越えるか考える
            else if (m_eyesight.IsBrow(Direction))
            {
                DirectionType direction = Lib.GetUDDirection(Position, m_player.Position);

                if (direction is DirectionType.Down)
                {
                    // 主人公が下にいる場合はそのまま進む。
                }
                else
                {
                    // 主人公が上にいる場合はジャンプする
                    switch (m_eyesight.GetJumpDistance(Direction))
                    {
                        case Eyesight.JumpDistance.ShortJump:

                            // 小ジャンプ
                            velocity.Y = m_shortJumpVelocity;
                            PlaySe(JumpSe);
                            // とりあえずジャンプ時に攻撃する
                            Shot();
                            break;

                        case Eyesight.JumpDistance.LongJump:

                            // 大ジャンプ
                            velocity.Y = m_jumpVelocity;
                            PlaySe(JumpSe);
                            // とりあえずジャンプ時に攻撃する
                            Shot();
                            break;

                        case Eyesight.JumpDistance.Retrun:

                            // 方向転換
                            Reverse();
                            break;
                    }
                }
            }
        }
        // 空中にいる
        else
        {
            // 主人公がいたら空中でも攻撃する
            if (!SkipAttack && m_eyesight.InSightOfPlayer())
            {
                Direction = Lib.GetLRDirection(Position, m_player.Position);
            }
        }

        velocity = Walk(delta, velocity, onFloor);

        if (onFloor && !_lastOnFloor)
        {
            PlaySe(TouchdownSe);
            // とりあえず着地後に攻撃する
            Shot();
        }

        _lastOnFloor = onFloor;
        return velocity;
    }

    private void Shot()
    {
        if (SkipAttack || Direction is DirectionType.Center)
        {
            return;
        }

        SetSkipAttack();

        if (Lib.GetPackedScene<PackedScene>("res://enemy/enemy_shot1.tscn") is PackedScene pack && pack.Instantiate() is Shot shot)
        {
            for (float i = 0f; i < 360f; i += 60f)
            {
                Vector2 shotDirection = Vector2.Right.Rotated(Mathf.DegToRad(i));
                _ = EmitSignal(Mob.SignalName.NodeSpawned, shot, this, GlobalPosition, shotDirection, 0f);
            }
        }
    }
}
