using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.mob;

/// <summary>
/// ブロック破壊マーカー
/// </summary>
public partial class BlockBreaker : Marker2D, IGameNode
{
    [Export]
    public float WaitTime { get; set; } = 0.4f;

    private VisibleOnScreenNotifier2D _visibleOnScreenNotifier;
    private bool _wait = false;
    private TileMapManager _tileMapManager = null;

    public override void _Ready()
    {
        AddToGroup(GameStageRoot.GameNodeGroup);
        AddToGroup(GameStageRoot.PhysicsProcessGroup);
        _visibleOnScreenNotifier = GetOwner<Node>().GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
    }

    public void InitializeNode()
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
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
        if (_wait)
        {
            return;
        }

        Vector2I step = (Vector2I)GlobalPosition;
        bool visible = _visibleOnScreenNotifier is not null && _visibleOnScreenNotifier.Visible;

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
