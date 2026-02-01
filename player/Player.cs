using Godot;
using tmfos.command.stage;
using tmfos.data;
using tmfos.item;
using tmfos.mob;
using tmfos.stage;
using tmfos.system;

namespace tmfos.player;

/// <summary>
/// プレーヤー
/// </summary>
public partial class Player : DurableMob, IStateful, ILight, ISwimAction, IClimbAction, ICrouchAction
{
    [Signal]
    public delegate void HudUpdatedEventHandler();

    [Signal]
    public delegate void MissedEventHandler();

    [Export]
    public float PlayerArmorRatio { get; set; } = 1f;

    private TileMapManager _tileMapManager;
    private CollisionShape2D _collision;
    private CollisionShape2D _burialCollision;
    private CollisionShape2D _eventFinderCollision;
    private ReferenceRect _referenceSwim;
    private ReferenceRect _referenceCrouch;
    private ReferenceRect _referenceNormal;
    private ItemData _itemData;
    private RayCast2D _rayCast;
    private Marker2D _shotLeft;
    private Marker2D _shotRight;
    private Marker2D _crouchShotLeft;
    private Marker2D _crouchShotRight;
    private Marker2D _step;
    private int _lastJumpDirection;
    private bool _burialSkip = false;
    private bool _lastOnFloor;
    private bool _standByClimb;
    private bool _coyote;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.LightSourceGroup);
        AddToGroup(StageRoot.StatefulGroup);
        AddToGroup(StageRoot.GameNodeGroup);
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");
        _burialCollision = GetNode<CollisionShape2D>("BurialArea/CollisionShape2D");
        _eventFinderCollision = GetNode<CollisionShape2D>("EventFinder/CollisionShape2D");
        _referenceSwim = GetNode<ReferenceRect>("ReferenceRect/ReferenceSwim");
        _referenceCrouch = GetNode<ReferenceRect>("ReferenceRect/ReferenceCrouch");
        _referenceNormal = GetNode<ReferenceRect>("ReferenceRect/ReferenceNormal");
        _rayCast = GetNode<RayCast2D>("RayCast2D");
        _shotLeft = GetNode<Marker2D>("ShotMarker/ShotLeft");
        _shotRight = GetNode<Marker2D>("ShotMarker/ShotRight");
        _crouchShotLeft = GetNode<Marker2D>("ShotMarker/CrouchShotLeft");
        _crouchShotRight = GetNode<Marker2D>("ShotMarker/CrouchShotRight");
        _step = GetNode<Marker2D>("BlockBreaker/Step");
    }

    public override void InitializeNode()
    {
        base.InitializeNode();
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        _ = Connect(SignalName.Missed, new(stageRoot, StageRoot.MethodName.Miss));
        _ = Connect(SignalName.HudUpdated, new(stageRoot, StageRoot.MethodName.UpdateHud));
        _tileMapManager = stageRoot.GetNode<TileMapManager>("TileMap");
        GameData gdata = GetNode<GameData>("/root/GameData");
        _itemData = gdata.GetItemData();
        Life = gdata.GetPlayerData().Life;
        PlayerArmorRatio = stageRoot.PlayerArmorRatio;
        Action = MobActionType.Walk;
        ItemSearchEffect();
        CollisionOnewayBlock(true, _itemData.Shoes);
    }

    public override void RemoveNode()
    {
        SetPhysicsProcess(false);
        PlaySe("player_miss");
        MobState = MobStateType.Sleep;
        _ = EmitSignal(SignalName.Missed);
    }

    public override void _PhysicsProcess(double delta)
    {
        Search = false;

        if (MobState is MobStateType.Sleep)
        {
            return;
        }

        // 死亡時・タイムアップ時は落ちるだけ
        if (MobState is MobStateType.Dead or MobStateType.Timeup)
        {
            Search = true;
            GravityOnly(delta);
            return;
        }

        // アクションの状態ごとに処理を行う
        switch (Action)
        {
            case MobActionType.Climb:

                ClimbAction(delta);
                break;

            case MobActionType.Crouch:

                CrouchAction(delta);
                break;

            case MobActionType.Swim:

                SwimAction(delta);
                break;

            case MobActionType.Walk:

                if (IsOnFloor())
                {
                    Vector2I step = (Vector2I)(GlobalPosition + _step.Position);
                    TileData tiledata = _tileMapManager.GetTileData(TileMapManager.GroundLayerPath, step);
                    bool ice = false;

                    if (tiledata is not null)
                    {
                        Variant v = tiledata.GetCustomData("IceBlock");

                        if (v.VariantType is Variant.Type.Bool)
                        {
                            ice = v.AsBool();
                        }
                    }

                    NormalAction(delta, ice && !_itemData.Shoes);
                }
                else
                {
                    AerialAction(delta);
                }

                break;
        }

        _ = EmitSignal(SignalName.HudUpdated);
    }

    public void ResetLifeTime(bool se = false)
    {
        base.ResetLifeTime();
        _ = EmitSignal(SignalName.HudUpdated);

        if (se)
        {
            PlaySe("player_reset_life_time");
        }
    }

    private void SwimAction(double delta)
    {
        // 上下左右に移動可能
        // ジャンプキーを押しっぱなしにすると加速
        Vector2 velocity = Velocity;
        Vector2 direction = GetInput();
        int signX = Mathf.Sign(direction.X);
        int signY = Mathf.Sign(direction.Y);
        _standByClimb = signY != 0;
        base.ChangeSprite("swim", Direction);

        if (signX == 0)
        {
            // a
            velocity.X = (float)Mathf.MoveToward(velocity.X, 0, m_swimAcceleration * delta);
        }
        else
        {
            // b
            velocity.X = (float)Mathf.MoveToward(velocity.X, signX * SwimMaxSpeed, m_swimAcceleration * delta);
            Direction = signX == -1 ? DirectionType.Left : DirectionType.Right;
        }

        if (signY == 0)
        {
            // a
            velocity.Y = (float)Mathf.MoveToward(velocity.Y, 0, m_swimAcceleration * delta);
        }
        else
        {
            // b
            velocity.Y = (float)Mathf.MoveToward(velocity.Y, signY * SwimMaxSpeed, m_swimAcceleration * delta);
        }

        Velocity = velocity;
        _ = MoveAndSlide();
    }

    private void ClimbAction(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = GetInput();
        int signX = Mathf.Sign(direction.X);
        int signY = Mathf.Sign(direction.Y);

        if (signX == 0)
        {
            // a
            velocity.X = (float)Mathf.MoveToward(velocity.X, 0, m_climbReductionAcceleration * delta);
        }
        else
        {
            // b
            velocity.X = (float)Mathf.MoveToward(velocity.X, signX * ClimbMaxSpeed, m_climbAcceleration * delta);
            Direction = signX == -1 ? DirectionType.Left : DirectionType.Right;
        }

        if (signY == 0)
        {
            // a
            velocity.Y = (float)Mathf.MoveToward(velocity.Y, 0, m_climbReductionAcceleration * delta);
        }
        else
        {
            // b
            velocity.Y = (float)Mathf.MoveToward(velocity.Y, signY * ClimbMaxSpeed, m_climbAcceleration * delta);
        }

        if (signX == 0 && signY == 0)
        {
            PauseSprite();
        }
        else
        {
            PlaySprite();
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            UnsetClimb();
            PlaySe(JumpSe);
        }

        _lastOnFloor = false;
        Velocity = velocity;
        _ = MoveAndSlide();
    }

    private void CrouchAction(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = GetInput();
        int signX = Mathf.Sign(direction.X);
        int signY = Mathf.Sign(direction.Y);

        if (signY == 0)
        {
            UnsetCrouch();
            return;
        }

        Direction = signX switch
        {
            -1 => DirectionType.Left,
            1 => DirectionType.Right,
            _ => Direction,
        };

        base.ChangeSprite("crouch", Direction);
        velocity.Y += m_gravity * (float)delta;

        if (Input.IsActionJustPressed("b") && !SkipAttack)
        {
            Shot(Direction is DirectionType.Left ? _crouchShotLeft : _crouchShotRight);
        }

        // しゃがんだ状態でジャンプはすり抜け
        if (Input.IsActionJustPressed("ui_accept"))
        {
            CollisionOnewayBlock(false, _itemData.Shoes);
            UnsetCrouch();
            RecoverCollisionOnewayBlock();
        }

        Velocity = velocity;
        _ = MoveAndSlide();
    }

    private async void RecoverCollisionOnewayBlock()
    {
        for (int i = 0; i < 8; i++)
        {
            _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        CollisionOnewayBlock(true, _itemData.Shoes);
    }

    private Vector2 GetInput()
    {
        Vector2 input = Vector2.Zero;
        bool up = Input.IsActionPressed("ui_up");
        bool down = Input.IsActionPressed("ui_down");
        bool left = Input.IsActionPressed("ui_left");
        bool right = Input.IsActionPressed("ui_right");

        if (left && !right)
        {
            input += Vector2.Left;
        }
        else if (!left && right)
        {
            input += Vector2.Right;
        }

        if (up && !down)
        {
            input += Vector2.Up;
        }
        else if (!up && down)
        {
            input += Vector2.Down;
        }

        return input;
    }

    public void SetCrouch()
    {
        Action = MobActionType.Crouch;
        ChangeSprite("crouch", Direction);
    }

    public void UnsetCrouch()
    {
        Action = MobActionType.Walk;
        ChangeSprite("walk", Direction);
    }

    private void NormalAction(double delta, bool iceMode)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = GetInput();
        int signX = Mathf.Sign(direction.X);
        int signY = Mathf.Sign(direction.Y);
        _standByClimb = signY != 0;
        Action = MobActionType.Walk;
        base.ChangeSprite("walk", Direction);
        velocity.Y += m_gravity * (float)delta;

        // 着地したら壁ジャンプの方向制限をリセットする
        _lastJumpDirection = 0;

        if (signX == 0)
        {
            PauseSprite();

            if (!iceMode)
            {
                velocity.X = (float)Mathf.MoveToward(velocity.X, 0, m_reductionAcceleration * delta);
            }
        }
        else
        {
            PlaySprite();
            Direction = signX == -1 ? DirectionType.Left : DirectionType.Right;

            if (iceMode)
            {
                // 1
                velocity.X = (float)Mathf.MoveToward(velocity.X, signX * MaxSpeed, m_acceleration * delta);
            }
            else
            {
                // 2
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
        }

        if (Direction is not DirectionType.Center)
        {
            if (Input.IsActionJustPressed("ui_accept"))
            {
                // ジャンプする
                // ここではジャンプのみ、
                // すり抜けはしゃがんだ状態で判定する
                _coyote = false;
                velocity.Y = m_jumpVelocity;
                PlaySe(JumpSe);
            }
            else if (signY == 1)
            {
                //床にいる状態から下キーでしゃがむ
                SetCrouch();
                velocity.X = 0f;
            }
        }

        if (!_lastOnFloor)
        {
            PlaySe(TouchdownSe);
            CollisionOnewayBlock(true, _itemData.Shoes);
        }

        // ショット
        if (Input.IsActionJustPressed("b") && Direction is not DirectionType.Center && !SkipAttack)
        {
            Shot(Direction is DirectionType.Left ? _shotLeft : _shotRight);
        }

        if (Input.IsActionPressed("ui_up"))
        {
            Search = true;
        }

        _lastOnFloor = true;
        Velocity = velocity;
        _ = MoveAndSlide();
    }

    private void AerialAction(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = GetInput();
        int signX = Mathf.Sign(direction.X);
        int signY = Mathf.Sign(direction.Y);
        _standByClimb = signY != 0;
        Action = MobActionType.Walk;
        base.ChangeSprite("walk", Direction);

        if (_lastOnFloor)
        {
            _coyote = true;
            UnsetCoyoteTime();
        }

        if (signX == 0)
        {
            velocity.X = (float)Mathf.MoveToward(velocity.X, 0, m_airReductionAcceleration * delta);
        }
        else
        {
            velocity.X = (float)Mathf.MoveToward(velocity.X, signX * AirMaxSpeed, m_airAcceleration * delta);
            Direction = signX == -1 ? DirectionType.Left : DirectionType.Right;
        }

        velocity.Y += m_gravity * (float)delta;

        if (Input.IsActionJustPressed("ui_accept") && (_rayCast.IsColliding() || _coyote))
        {
            // 空中でもRayCastが接触している場合とコヨーテタイム中はジャンプする
            _coyote = false;
            velocity.Y = m_jumpVelocity;
            PlaySe(JumpSe);
        }
        else if (Input.IsActionJustPressed("ui_accept") && _itemData.WallJump && IsOnWall() && _lastJumpDirection != signX)
        {
            // 壁に接触しており、
            // 前回壁ジャンプした方向と別方向なら、ジャンプ可能
            _lastJumpDirection = signX;

            // ジャンプする
            // ここではジャンプのみ、すり抜けはしゃがんだ状態で判定する
            velocity.Y = m_jumpVelocity;
            PlaySe(JumpSe);
        }
        else if (Input.IsActionJustReleased("ui_accept") && velocity.Y < m_shortJumpVelocity)
        {
            // 空中でジャンプキーが離され、
            // ジャンプの速度が小ジャンプの初速より小さければ、その位置から小ジャンプ
            velocity.Y = m_shortJumpVelocity;
        }

        // ショット
        if (Input.IsActionJustPressed("b") && Direction is not DirectionType.Center && !SkipAttack)
        {
            Shot(Direction is DirectionType.Left ? _shotLeft : _shotRight);
        }

        if (Input.IsActionPressed("ui_up"))
        {
            Search = true;
        }

        PauseSprite();
        _lastOnFloor = false;
        Velocity = velocity;
        _ = MoveAndSlide();
    }

    private void Shot(Marker2D offset)
    {
        SetSkipAttack();
        Vector2 shotDirection = Direction is DirectionType.Left ? Vector2.Left : Vector2.Right;

        if (Lib.GetPackedScene<PackedScene>("res://player/player_shot1.tscn") is PackedScene pack && pack.Instantiate() is Shot shot)
        {
            shot.Penetration = _itemData.Penetration;
            shot.Weapon = _itemData.Weapon;
            _ = EmitSignal(Mob.SignalName.NodeSpawned, shot, this, GlobalPosition + offset.Position, shotDirection, 0f);
        }
    }

    public override void Burialed(Node2D body)
    {
        if (_burialSkip)
        {
            return;
        }

        Dead();
    }

    public void SetClimb()
    {
        if (!_standByClimb || SkipDamage)
        {
            return;
        }

        if (Action is MobActionType.Climb)
        {
            return;
        }

        _standByClimb = false;
        Action = MobActionType.Climb;
        Velocity = Vector2.Zero;
        ChangeSprite("climb", DirectionType.Center);
    }

    public void UnsetClimb()
    {
        if (Action is not MobActionType.Climb)
        {
            return;
        }

        _standByClimb = false;
        Action = MobActionType.Walk;
        ChangeSprite("walk", Direction);
    }

    public void SetSwim()
    {
        if (!_itemData.Swim)
        {
            Dead();
            return;
        }

        if (Action is MobActionType.Swim)
        {
            return;
        }

        Action = MobActionType.Swim;
        Vector2 velocity = Velocity;
        velocity = velocity.Clamp(-SwimMaxSpeed, SwimMaxSpeed);
        Velocity = velocity;
        ChangeSprite("swim", Direction);
    }

    public void UnsetSwim()
    {
        if (Action is not MobActionType.Swim)
        {
            return;
        }

        Action = MobActionType.Walk;
        ChangeSprite("walk", Direction);
        Vector2 velocity = Velocity;
        velocity.Y = m_shortJumpVelocity;
        Velocity = velocity;
    }

    public void StateSave()
    {
    }

    public void StateLoad()
    {
    }

    public override void Damaged()
    {
        base.Damaged();
        PlaySe(DamageSe);

        // ハシゴでダメージを受けた場合、ハシゴは解除される
        if (Action is MobActionType.Climb)
        {
            UnsetClimb();
            Action = MobActionType.Walk;
        }
    }

    public override void FullRecovered()
    {
        PlaySe("player_full_recovered");
    }

    public override void Recovered()
    {
        PlaySe("player_recovered");
    }

    public override void Resurrected()
    {
        base.Resurrected();
        PlaySe("player_resurrected");
    }

    public override void ChangeSprite(string animation, DirectionType direction)
    {
        ReferenceRect reference = _referenceNormal;

        if (animation == "crouch")
        {
            reference = _referenceCrouch;
        }
        else if (animation == "swim")
        {
            reference = _referenceSwim;
        }

        Shape2D shape = _collision.Shape;

        if (shape is RectangleShape2D rect)
        {
            rect.Size = reference.Size;
            _collision.Position = reference.Position + reference.PivotOffset;
        }

        Shape2D eventFinderShape = _eventFinderCollision.Shape;

        if (eventFinderShape is RectangleShape2D eventFinderRect)
        {
            eventFinderRect.Size = reference.Size;
            _eventFinderCollision.Position = reference.Position + reference.PivotOffset;
        }

        Shape2D burialShape = _burialCollision.Shape;

        if (burialShape is RectangleShape2D burialRect)
        {
            burialRect.Size = reference.Size * 0.7f;
            _burialCollision.Position = reference.Position + reference.PivotOffset;
            _burialSkip = true;
            RecoverBurial();
        }

        base.ChangeSprite(animation, direction);
    }

    private async void UnsetCoyoteTime()
    {
        for (int i = 0; i < 6; i++)
        {
            _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        _coyote = false;
    }

    private async void RecoverBurial()
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _burialSkip = false;
    }

    private void ItemSearchEffect()
    {
        if (_itemData.Search)
        {
            GetNode<Node2D>("SearchNode").Show();
        }
    }

    public void ManageItem(ItemType item, bool flag)
    {
        switch (item)
        {
            case ItemType.Shoes:

                _itemData.Shoes = flag;
                break;

            case ItemType.Swim:

                _itemData.Swim = flag;
                break;

            case ItemType.WallJump:

                _itemData.WallJump = flag;
                break;

            case ItemType.Penetration:

                _itemData.Penetration = flag;
                break;

            case ItemType.Lamp:

                _itemData.Lamp = flag;
                EnableLight();
                break;

            case ItemType.Search:

                _itemData.Search = flag;
                ItemSearchEffect();
                break;

            case ItemType.Armor:

                _itemData.Armor = Mathf.Clamp(_itemData.Armor + (flag ? 1 : -1), 0, 150);
                break;

            case ItemType.Weapon:

                _itemData.Weapon = Mathf.Clamp(_itemData.Weapon + (flag ? 1 : -1), 0, 150);
                break;
        }
    }

    public void EnableLight()
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();

        if (!stageRoot.IsDarkZone)
        {
            return;
        }

        if (_itemData.Lamp)
        {
            GetNode<PointLight2D>("Light/PointLight2D").Hide();
            GetNode<PointLight2D>("Light/PointLight2DLampItem").Show();
            return;
        }

        GetNode<PointLight2D>("Light/PointLight2D").Show();
        GetNode<PointLight2D>("Light/PointLight2DLampItem").Hide();
    }

    public override void AddDurability(int value)
    {
        if (value < 0)
        {
            // 最低でもダメージ1は受ける
            value = Mathf.Clamp(value + (int)(_itemData.Armor * PlayerArmorRatio), int.MinValue, -1);
        }

        base.AddDurability(value);
        GetNode<GameData>("/root/GameData").GetPlayerData().Life = Life;
        _ = EmitSignal(SignalName.HudUpdated);
    }

    public override void SetDurability(int value)
    {
        base.SetDurability(value);
        GetNode<GameData>("/root/GameData").GetPlayerData().Life = Life;
        _ = EmitSignal(SignalName.HudUpdated);
    }
}
