using Godot;
using Godot.Collections;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 敵出現ポイント
/// </summary>
public partial class EnemySpawner : Node2D, IGameNode, ISpawner
{
    public enum EnemyResourceName
    {
        ThreeMeterEnemy,
        NekomimiMaidEnemy,
        CatEnemy,
        UnmoEnemy,
        RocketMaidEnemy,
        BirdEnemy,
        FallingIcicleEnemy,
        FallingBlock1Enemy,
        FallingBlock2Enemy,
        FallingBlock3Enemy,
        FallingBlock4Enemy,
        FallingBlock5Enemy,
        FallingBlock6Enemy,
        DrillShotUp,
        DrillShotDown,
        DrillShotLeft,
        DrillShotRight,
        BigDrillShotUp,
        BigDrillShotDown,
        BigDrillShotLeft,
        BigDrillShotRight,
    }

    private Dictionary<string, string> EnemyResourceList = new() {
        {"ThreeMeterEnemy","res://enemy/three_meter_enemy.tscn"},
        {"NekomimiMaidEnemy","res://enemy/nekomimi_maid_c{0:#}_enemy.tscn"},
        {"CatEnemy","res://enemy/cat_enemy.tscn"},
        {"UnmoEnemy","res://enemy/unmo_enemy.tscn"},
        {"RocketMaidEnemy","res://enemy/rocket_maid_enemy.tscn"},
        {"BirdEnemy","res://enemy/bird_enemy.tscn"},
        {"FallingIcicleEnemy","res://enemy/falling_icicle_enemy.tscn"},
        {"FallingBlock1Enemy","res://enemy/falling_block1_enemy.tscn"},
        {"FallingBlock2Enemy","res://enemy/falling_block2_enemy.tscn"},
        {"FallingBlock3Enemy","res://enemy/falling_block3_enemy.tscn"},
        {"FallingBlock4Enemy","res://enemy/falling_block4_enemy.tscn"},
        {"FallingBlock5Enemy","res://enemy/falling_block5_enemy.tscn"},
        {"FallingBlock6Enemy","res://enemy/falling_block6_enemy.tscn"},
        {"DrillShotUp","res://enemy/drill_shot_up.tscn"},
        {"DrillShotDown","res://enemy/drill_shot_down.tscn"},
        {"DrillShotLeft","res://enemy/drill_shot_left.tscn"},
        {"DrillShotRight","res://enemy/drill_shot_right.tscn"},
        {"BigDrillShotUp","res://enemy/big_drill_shot_up.tscn"},
        {"BigDrillShotDown","res://enemy/big_drill_shot_down.tscn"},
        {"BigDrillShotLeft","res://enemy/big_drill_shot_left.tscn"},
        {"BigDrillShotRight","res://enemy/big_drill_shot_right.tscn"},
    };

    [Signal]
    public delegate void NodeSpawnedEventHandler(Node node, Node spawner, Vector2 position, Vector2 direction, double lifeTime);

    [Export]
    public EnemyResourceName EnemyName { get; set; } = EnemyResourceName.NekomimiMaidEnemy;

    [Export]
    public float EnemyLifeTime { get; set; } = 10f;

    [Export]
    public bool SingleSpawn { get; set; } = false;

    [Export]
    public bool ManualMode { get; set; } = false;

    private Marker2D _marker;
    private Player _player;
    private bool _spawned = false;

    public override void _Ready()
    {
        AddToGroup(StageRoot.GameNodeGroup);
        _marker = GetNode<Marker2D>("Marker2D");
        _ = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").Connect(VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new(this, MethodName.SpawnEnemy));

        if (ManualMode)
        {
            GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").SetBlockSignals(true);
        }
    }

    public void InitializeNode()
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        _ = Connect(SignalName.NodeSpawned, new(stageRoot, StageRoot.MethodName.SpawnNode));
        _player = stageRoot.GetNode<Player>("%Player");
    }

    public void FinalizeNode()
    {
    }

    public void RemoveNode()
    {
        SetPhysicsProcess(false);
        QueueFree();
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    public void SpawnEnemy()
    {
        if (SingleSpawn && _spawned)
        {
            return;
        }

        RandomNumberGenerator random = new();
        string resource = string.Format(EnemyResourceList[EnemyName.ToString()], random.RandiRange(1, 8));

        if (Lib.GetPackedScene<PackedScene>(resource) is PackedScene pack && pack.Instantiate() is Node enemy)
        {
            _ = EmitSignal(SignalName.NodeSpawned, enemy, this, GlobalPosition + _marker.Position, Vector2.Zero, EnemyLifeTime);
        }
    }

    public void SetSpawned()
    {
        _spawned = true;
    }

    public void ResetSpawned()
    {
        _spawned = false;
    }

    public Callable GetSignalMethod()
    {
        return new(this, MethodName.ResetSpawned);
    }
}
