using Godot;
using tmfos.mob;
using tmfos.stage;
using tmfos.system;

namespace tmfos.player;

/// <summary>
/// 前面ブロック透過
/// </summary>
public partial class VeilLight : Node2D, IGameNode
{
    private TileMapManager _tileMapManager = null;

    public override void _Ready()
    {
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
        Vector2I step = (Vector2I)GlobalPosition;
        _tileMapManager.VisibleTileMapLayer(TileMapManager.VeilLayerPath, _tileMapManager.GetTileData(TileMapManager.VeilLayerPath, step) is null);
    }
}
