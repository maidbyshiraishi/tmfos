using Godot;
using tmfos.mob;
using tmfos.player;

namespace tmfos.enemy;

/// <summary>
/// 敵の視界
/// </summary>
public partial class Eyesight : Node2D
{
    /// <summary>
    /// ジャンプ距離
    /// </summary>
    public enum JumpDistance
    {
        ShortJump,
        LongJump,
        Brodie,
        Retrun
    }

    private RayCast2D _leftDown1;
    private RayCast2D _leftDown2;
    private RayCast2D _leftDown3;
    private RayCast2D _rightDown1;
    private RayCast2D _rightDown2;
    private RayCast2D _rightDown3;
    private RayCast2D _canJumpLeft1;
    private RayCast2D _canJumpLeft2;
    private RayCast2D _canJumpRight1;
    private RayCast2D _canJumpRight2;
    private RayCast2D _shortLeft;
    private RayCast2D _shortRight;
    private RayCast2D _longLeft;
    private RayCast2D _longRight;

    public override void _Ready()
    {
        _leftDown1 = GetNode<RayCast2D>("LeftDown1");
        _leftDown2 = GetNode<RayCast2D>("LeftDown2");
        _leftDown3 = GetNode<RayCast2D>("LeftDown3");
        _rightDown1 = GetNode<RayCast2D>("RightDown1");
        _rightDown2 = GetNode<RayCast2D>("RightDown2");
        _rightDown3 = GetNode<RayCast2D>("RightDown3");
        _canJumpLeft1 = GetNode<RayCast2D>("CanJumpLeft1");
        _canJumpLeft2 = GetNode<RayCast2D>("CanJumpLeft2");
        _canJumpRight1 = GetNode<RayCast2D>("CanJumpRight1");
        _canJumpRight2 = GetNode<RayCast2D>("CanJumpRight2");
        _shortLeft = GetNode<RayCast2D>("ShortLeft");
        _shortRight = GetNode<RayCast2D>("ShortRight");
        _longLeft = GetNode<RayCast2D>("LongLeft");
        _longRight = GetNode<RayCast2D>("LongRight");
    }

    /// <summary>
    /// プレーヤーの攻撃が近距離に到達している
    /// </summary>
    /// <param name="direction">方向</param>
    /// <returns>true or false</returns>
    public bool IsAvoidPlayerShot(DirectionType direction)
    {
        return (direction is DirectionType.Left && _shortLeft.IsColliding() && _shortLeft.GetCollider() is not null and PlayerShot1) || (direction is DirectionType.Right && _shortRight.IsColliding() && _shortRight.GetCollider() is not null and PlayerShot1);
    }

    /// <summary>
    /// プレーヤーが攻撃範囲にいる
    /// </summary>
    /// <returns>true or false</returns>
    public bool InSightOfPlayer()
    {
        return (_longLeft.IsColliding() && _longLeft.GetCollider() is not null and Player) || (_longRight.IsColliding() && _longRight.GetCollider() is not null and Player);
    }

    /// <summary>
    /// 両方壁に囲まれているか
    /// </summary>
    /// <returns>true or false</returns>
    public bool IsEncloseWall()
    {
        return _shortLeft.IsColliding() && _shortLeft.GetCollider() is not null and TileMapLayer && _shortRight.IsColliding() && _shortRight.GetCollider() is not null and TileMapLayer;
    }

    /// <summary>
    /// 壁が迫っているか
    /// </summary>
    /// <param name="direction">方向</param>
    /// <returns>true or false</returns>
    public bool IsFaceToWall(DirectionType direction)
    {
        return (direction is DirectionType.Left && _shortLeft.IsColliding() && _shortLeft.GetCollider() is not null and TileMapLayer) || (direction == DirectionType.Right && _shortRight.IsColliding() && _shortRight.GetCollider() is not null and TileMapLayer);
    }

    /// <summary>
    /// 頭上が開いているか
    /// </summary>
    /// <param name="direction">方向</param>
    /// <returns>true or false</returns>
    public bool IsJumpWall(DirectionType direction)
    {
        return (!_canJumpLeft1.IsColliding() && !_canJumpLeft2.IsColliding() && direction is DirectionType.Left) || (!_canJumpRight1.IsColliding() && !_canJumpRight2.IsColliding() && direction is DirectionType.Right);
    }

    /// <summary>
    /// 崖があるか
    /// </summary>
    /// <param name="direction">方向</param>
    /// <returns>true or false</returns>
    public bool IsBrow(DirectionType direction)
    {
        return (!_leftDown1.IsColliding() && direction is DirectionType.Left) || (!_rightDown1.IsColliding() && direction is DirectionType.Right);
    }

    /// <summary>
    /// ジャンプ距離を決める
    /// </summary>
    /// <param name="direction">方向</param>
    /// <returns>JumpDistance</returns>
    public JumpDistance GetJumpDistance(DirectionType direction)
    {
        if (direction is DirectionType.Left)
        {
            bool leftDown2 = _leftDown2.IsColliding();
            bool leftDown3 = _leftDown3.IsColliding();

            // 頭上が開いてない場合
            // または対岸が存在しない場合は引き返す
            if (!leftDown2 && !leftDown3)
            {
                return JumpDistance.Brodie;
            }
            else if (leftDown2)
            {
                return JumpDistance.ShortJump;
            }
            else if (leftDown3)
            {
                return JumpDistance.LongJump;
            }
        }
        else if (direction is DirectionType.Right)
        {
            bool rightDown2 = _rightDown2.IsColliding();
            bool rightDown3 = _rightDown3.IsColliding();

            // 頭上が開いてない場合
            // または対岸が存在しない場合は引き返す
            if (!rightDown2 && !rightDown3)
            {
                return JumpDistance.Brodie;
            }
            else if (rightDown2)
            {
                return JumpDistance.ShortJump;
            }
            else if (rightDown3)
            {
                return JumpDistance.LongJump;
            }
        }

        return JumpDistance.Retrun;
    }
}
