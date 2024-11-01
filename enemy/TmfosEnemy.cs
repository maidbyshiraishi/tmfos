using Godot;
using Godot.Collections;
using tmfos.mob;
using tmfos.stage;
using tmfos.system;

namespace tmfos.enemy;

/// <summary>
/// 宇宙から来たメイドボス
/// </summary>
public partial class TmfosEnemy : PathFollowEnemy
{
    [Signal]
    public delegate void NodeSpawnedEventHandler(Node node, Node spawner, Vector2 position, Vector2 direction, double lifeTime);

    [Export]
    public Marker2D MerkerPosition { get; set; }

    /// <summary>
    /// テレポート先グループ名
    /// </summary>
    [Export]
    public string TeleportPositionGroupName { get; set; } = "TeleportPosition";

    [Export]
    public double TeleportTime { get; set; } = 0.5f;

    private Array<Node> _teleportPosition;
    private bool _tereportReserved = false;

    public override void _Ready()
    {
        base._Ready();
        GetNode<TextureProgressBar>("%HUD/BossLife").MaxValue = Life;
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }

    public override void InitializeNode()
    {
        base.InitializeNode();
        // 攻撃力強化の対象外
        m_attackCorrection = 0;
        _teleportPosition = GetTree().GetNodesInGroup(TeleportPositionGroupName);
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        _ = Connect(SignalName.NodeSpawned, new Callable(stageRoot, StageRoot.MethodName.SpawnNode));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (MobState is not MobStateType.Dead and not MobStateType.Timeup && MerkerPosition != null)
        {
            GlobalPosition = MerkerPosition.GlobalPosition;
        }

        base._PhysicsProcess(delta);
    }

    public override void Dead()
    {
        base.Dead();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = 0;
    }

    public override void Damaged()
    {
        PlaySprite("laser");
        PackedScene decoration = Lib.GetPackedScene("res://decoration/excitation.tscn");
        Node decorationNode = decoration.Instantiate();
        _ = EmitSignal(Mob.SignalName.NodeSpawned, decorationNode, this, GlobalPosition, Vector2.Zero, 0f);
        _ = decorationNode.Connect(Node.SignalName.TreeExited, new Callable(this, MethodName.Laser));

        base.Damaged();
        ReadyForWarp();
        GetNode<TextureProgressBar>("%HUD/BossLife").Value = Life;
    }

    protected async void ReadyForWarp()
    {
        if (_tereportReserved)
        {
            return;
        }

        if (TeleportTime >= 0.05f)
        {
            _tereportReserved = true;
            _ = await ToSignal(GetTree().CreateTimer(TeleportTime), Timer.SignalName.Timeout);
        }

        _tereportReserved = false;
        Warp();
    }

    private void Warp()
    {
        if (_teleportPosition.Count == 0)
        {
            return;
        }

        RandomNumberGenerator random = new();
        int no = random.RandiRange(0, _teleportPosition.Count - 1);

        if (_teleportPosition[no] is Marker2D merker)
        {
            MerkerPosition = merker;
        }
    }

    private void Laser()
    {
        using PackedScene laser = Lib.GetPackedScene("res://enemy/enemy_laser.tscn");
        Vector2 shotDirection = m_player.GlobalPosition - GlobalPosition;
        Shot shotNode = (Shot)laser.Instantiate();
        shotNode.Penetration = true;
        _ = EmitSignal(Mob.SignalName.NodeSpawned, shotNode, this, GlobalPosition, shotDirection, 0f);
    }
}
