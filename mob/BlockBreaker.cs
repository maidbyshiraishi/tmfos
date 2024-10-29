using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.mob;

/// <summary>
/// ブロック破壊マーカー
/// </summary>
public partial class BlockBreaker : Marker2D, IGameNode
{
    [Export]
    public float WaitTime { get; set; } = 0.4f;

    [Export]
    public VisibleOnScreenNotifier2D VisibleOnScreenNotifier { get; set; }

    private bool _wait = false;
    private TileMapManager _tileMapManager = null;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.GameNodeGroup);
        AddToGroup(StageRoot.PhysicsProcessGroup);
    }

    public void InitializeNode()
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        _tileMapManager = stageRoot.GetNode<TileMapManager>("TileMap");
    }

    public void FinalizeNode()
    {
    }

    public void RemoveNode()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_wait)
        {
            return;
        }

        Vector2I step = (Vector2I)GlobalPosition;
        bool visible = VisibleOnScreenNotifier is not null && VisibleOnScreenNotifier.Visible;

        if (_tileMapManager.BlockCollided(step, visible))
        {
            Wait();
        }
    }

    private async void Wait()
    {
        if (WaitTime >= 0.05f)
        {
            _wait = true;
            _ = await ToSignal(GetTree().CreateTimer(WaitTime), Timer.SignalName.Timeout);
        }

        _wait = false;
    }
}
