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
        AddToGroup(GameStageRoot.GameNodeGroup);
        AddToGroup(GameStageRoot.PhysicsProcessGroup);
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

    public override void _PhysicsProcess(double delta) => _tileMapManager.OpenObservationHole(_tileMapManager.GetTileData(TileMapManager.VeilLayerPath, (Vector2I)GlobalPosition) is not null, GlobalPosition);
}
