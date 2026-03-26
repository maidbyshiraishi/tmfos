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

    /// <summary>
    /// テレポート先グループ名
    /// </summary>
    [Export]
    public string TeleportPositionGroupName { get; set; } = "TeleportPosition";

    [Export]
    public double TeleportTime { get; set; } = 0.5f;

    private Array<Node> _teleportPosition;
    private bool _tereportReserved = false;
    private Marker2D _merkedPosition;

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
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        NodeSpawned += stageRoot.SpawnNode;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (MobState is not MobStateType.Dead and not MobStateType.Timeup && _merkedPosition != null)
        {
            GlobalPosition = _merkedPosition.GlobalPosition;
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

        if (Lib.GetPackedScene("res://decoration/excitation.tscn") is PackedScene pack && pack.Instantiate() is Node decoration)
        {
            _ = EmitSignal(Mob.SignalName.NodeSpawned, decoration, this, GlobalPosition, Vector2.Zero, 0f);
            decoration.TreeExited += Laser;
        }

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
            _merkedPosition = merker;
        }
    }

    private void Laser()
    {
        Vector2 shotDirection = m_player.GlobalPosition - GlobalPosition;

        if (Lib.GetPackedScene("res://enemy/enemy_laser2.tscn") is PackedScene pack && pack.Instantiate() is Shot shot)
        {
            shot.Penetration = true;
            _ = EmitSignal(Mob.SignalName.NodeSpawned, shot, this, GlobalPosition, shotDirection, 0f);
        }
    }
}
