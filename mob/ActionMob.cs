using Godot;
using tmfos.stage;

namespace tmfos.mob;

/// <summary>
/// プレーヤーなどアクションを行うキャラクター
/// </summary>
public partial class ActionMob : Mob
{
    [Export]
    public DirectionType Direction { get; set; } = DirectionType.Center;

    [Export]
    public bool Search { get; set; } = false;

    /// <summary>
    /// 助走距離
    /// 3ブロックで最大速度に到達する
    /// </summary>
    [Export]
    public float Approach { get; set; } = 96f;

    /// <summary>
    /// 最大速度
    /// </summary>
    [Export]
    public float MaxSpeed { get; set; } = 350f;

    [Export]
    public float ReductionApproach { get; set; } = 16f;

    [Export]
    public float AirApproach { get; set; } = 64f;

    [Export]
    public float AirMaxSpeed { get; set; } = 350f;

    [Export]
    public float AirReductionApproach { get; set; } = 64f;

    [Export]
    public float ClimbApproach { get; set; } = 64f;

    [Export]
    public float ClimbMaxSpeed { get; set; } = 200f;

    [Export]
    public float ClimbReductionApproach { get; set; } = 8f;

    [Export]
    public float SwimApproach { get; set; } = 96f;

    [Export]
    public float SwimMaxSpeed { get; set; } = 150f;

    [Export]
    public float SwimReductionApproach { get; set; } = 128f;

    [Export]
    public float JumpTime { get; set; } = 0.5f;

    [Export]
    public float ShortJumpHeight { get; set; } = 64f;

    [Export]
    public float JumpHeight { get; set; } = 128f;

    [Export]
    public MobActionType Action { get; set; } = MobActionType.Walk;

    [Export]
    public int Attack { get; set; } = 20;

    [Export]
    public bool SkipAttack { get; set; } = false;

    [Export]
    public double SkipAttackTime { get; set; } = 0.5f;

    [Export]
    public string JumpSe { get; set; }

    [Export]
    public string TouchdownSe { get; set; }

    protected float m_acceleration;
    protected float m_reductionAcceleration;
    protected float m_airAcceleration;
    protected float m_airReductionAcceleration;
    protected float m_climbAcceleration;
    protected float m_climbReductionAcceleration;
    protected float m_swimAcceleration;
    protected float m_swimReductionAcceleration;
    protected float m_jumpVelocity;
    protected float m_shortJumpVelocity;
    protected float m_gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.PhysicsProcessGroup);

        // 基本公式
        // v = v0 + a * t
        // x = v0 * t + (a * pow(t, 2)) / 2
        // pow(v, 2) - pow(v0, 2) = 2 * a * x

        // pow(v, 2) - pow(v0, 2) = 2 * a * x
        // v0 = 0;
        // pow(v, 2) = 2 * a * x
        // pow(v, 2) / (2 * x) = a
        m_acceleration = Mathf.Pow(MaxSpeed, 2f) / (Approach * 2f);
        m_airAcceleration = Mathf.Pow(AirMaxSpeed, 2f) / (AirApproach * 2f);
        m_climbAcceleration = Mathf.Pow(ClimbMaxSpeed, 2f) / (ClimbApproach * 2f);
        m_swimAcceleration = Mathf.Pow(SwimMaxSpeed, 2f) / (SwimApproach * 2f);

        m_reductionAcceleration = Mathf.Pow(MaxSpeed, 2f) / (ReductionApproach * 2f);
        m_airReductionAcceleration = Mathf.Pow(AirMaxSpeed, 2f) / (AirReductionApproach * 2f);
        m_climbReductionAcceleration = Mathf.Pow(ClimbMaxSpeed, 2f) / (ClimbReductionApproach * 2f);
        m_swimReductionAcceleration = Mathf.Pow(SwimMaxSpeed, 2f) / (SwimReductionApproach * 2f);

        // x = v0 * t + (a * pow(t, 2)) / 2
        // v0 = 0
        // a = g
        // x = h
        // 2 * h = g * pow(t, 2)
        // 2 * h / pow(t, 2) = g
        // 座標は下がプラス
        m_gravity = 2f * JumpHeight / Mathf.Pow(JumpTime, 2);

        // pow(v, 2) - pow(v0, 2) = 2 * a * x
        // a = g
        // v0 = 0
        // pow(v, 2) = 2 * g * x
        // v = sqrt(2 * g * x)
        // 座標は上がマイナス
        m_jumpVelocity = -Mathf.Sqrt(2f * m_gravity * JumpHeight);
        m_shortJumpVelocity = -Mathf.Sqrt(2f * m_gravity * ShortJumpHeight);
    }

    public override void _PhysicsProcess(double delta)
    {
        GravityOnly(delta);
    }

    public virtual void Burialed(Node2D body)
    {
    }

    /// <summary>
    /// 一方通行のすり抜けを制御する
    /// すり抜けない場合はtrue、すり抜ける場合はfalseを指定する
    /// 対象のアイテムを所有する場合、雲に乗れる
    /// </summary>
    /// <param name="flag">フラグ</param>
    /// <param name="cloud">雲乗りアイテムの有無</param>
    protected void CollisionOnewayBlock(bool flag, bool cloud = true)
    {
        // 雲乗りアイテムの有無によって雲のすり抜けは変わる
        SetCollisionMaskValue(10, flag && cloud);
        SetCollisionMaskValue(11, flag);
    }

    protected void CollisionTilemap(bool flag)
    {
        // レイヤ(*は死亡時に通過)
        //なし: 検出される必要のない何でもレイヤー(BurialAreaやEventFinderは一方的に相手を検出するだけ)
        //  1*: tilemap(タイルマップ、BurialAreaで検出し壁埋まり対策を行う)
        //  2*: tilemap_others(その他マップ構成物 弾をはじく, 壊れる床, 移動する床, ベルトコンベア)
        //  3*: tilemap_pass_through(その他マップ構成物 弾を通す, ハシゴ, ダメージゾーン)
        //  4 : player(プレーヤー)
        //  5 : player_shot(プレーヤー弾)
        //  6 : enemy(敵)
        //  7 : enemy_shot(敵弾)
        //  8 : search(探索エリア)
        // 10*: oneway_with_item_shoes(ブーツブロック一方通行ブロック)
        // 11*: oneway_block(一方通行ブロック)
        // 13 : item_search_effect(虫眼鏡アイテム効果)
        // 17 : event(アイテム、回復アイテム、宝箱、扉、ボス扉、ワープ、会話)
        SetCollisionMaskValue(1, flag);
        SetCollisionMaskValue(2, flag);
        SetCollisionMaskValue(3, flag);
        SetCollisionMaskValue(10, flag);
        SetCollisionMaskValue(11, flag);
    }

    protected void Reverse()
    {
        if (Direction is DirectionType.Left)
        {
            Direction = DirectionType.Right;
        }
        else if (Direction is DirectionType.Right)
        {
            Direction = DirectionType.Left;
        }
    }

    protected Vector2 Walk(double delta, Vector2 velocity, bool onFloor)
    {
        int signX = (int)Direction;
        PlaySprite();

        if (onFloor)
        {
            // 1
            if (-signX == Mathf.Sign(velocity.X))
            {
                // a
                velocity.X = (float)Mathf.MoveToward(velocity.X, 0, (m_reductionAcceleration + m_acceleration) * delta);
            }
            else
            {
                // b
                velocity.X = (float)Mathf.MoveToward(velocity.X, signX * MaxSpeed, m_acceleration * delta);
            }
        }
        else
        {
            // 2
            if (signX == 0)
            {
                // a
                velocity.X = (float)Mathf.MoveToward(velocity.X, 0, m_airReductionAcceleration * delta);
            }
            else
            {
                // b
                velocity.X = (float)Mathf.MoveToward(velocity.X, signX * AirMaxSpeed, m_airAcceleration * delta);
            }

            PauseSprite();
        }

        return velocity;
    }

    protected virtual void GravityOnly(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.X = 0f;
        velocity.Y += m_gravity * (float)delta;
        Velocity = velocity;
        _ = MoveAndSlide();
    }

    protected async void SetSkipAttack()
    {
        if (SkipAttack)
        {
            return;
        }

        if (SkipAttackTime >= 0.05f)
        {
            SkipAttack = true;
            _ = await ToSignal(GetTree().CreateTimer(SkipAttackTime), Timer.SignalName.Timeout);
        }

        SkipAttack = false;
    }
}
