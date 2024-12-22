using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.mob;

/// <summary>
/// 行動タイプ変更ノード
/// </summary>
public partial class ActionChange : Node2D, IGameNode
{
    private DurableMob _changeTarget;
    private TileMapManager _tileMapManager = null;
    private bool _ladder;
    private bool _water;
    private Marker2D _markerHead;
    private Marker2D _marker;

    public override void _Ready()
    {
        AddToGroup(StageRoot.GameNodeGroup);
        AddToGroup(StageRoot.PhysicsProcessGroup);
        _markerHead = GetNodeOrNull<Marker2D>("Head");
        _marker = GetNode<Marker2D>("Marker2D");

        if (GetParent() is DurableMob dmob)
        {
            _changeTarget = dmob;
        }
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
        bool tiledataHead;
        bool tiledata;

        if (_markerHead is null)
        {
            Vector2I step = (Vector2I)(GlobalPosition + _marker.Position);
            tiledataHead = _tileMapManager.GetCustomDataAsBool(TileMapManager.ForegroundLayerPath, step, "WaterArea");
            tiledata = _tileMapManager.GetCustomDataAsBool(TileMapManager.BackgroundLayerPath, step, "LadderArea");
        }
        else
        {
            Vector2I stepHead = (Vector2I)(GlobalPosition + _markerHead.Position);
            Vector2I step = (Vector2I)(GlobalPosition + _marker.Position);
            tiledataHead = _tileMapManager.GetCustomDataAsBool(TileMapManager.ForegroundLayerPath, stepHead, "WaterArea");
            tiledata = _tileMapManager.GetCustomDataAsBool(TileMapManager.BackgroundLayerPath, step, "LadderArea");
        }

        if (tiledata)
        {
            EnterClimb();
        }
        else if (_ladder)
        {
            ExitClimb();
        }

        if (tiledataHead)
        {
            EnterWater();
        }
        else if (_water)
        {
            ExitWater();
        }
    }

    public void EnterWater()
    {
        _water = true;

        if (_changeTarget.MobState is MobStateType.Normal && _changeTarget.Action is not MobActionType.Climb && _changeTarget is ISwimAction swim)
        {
            swim.SetSwim();
        }
    }

    public void ExitWater()
    {
        _water = false;

        if (_changeTarget.MobState is MobStateType.Normal && _changeTarget is ISwimAction swim)
        {
            swim.UnsetSwim();
        }
    }

    public void EnterClimb()
    {
        _ladder = true;

        if (_changeTarget.MobState is MobStateType.Normal && _changeTarget is IClimbAction climb)
        {
            climb.SetClimb();
        }
    }

    public void ExitClimb()
    {
        _ladder = false;

        if (_changeTarget.MobState is not MobStateType.Normal)
        {
            return;
        }

        if (_water && _changeTarget is ISwimAction swim)
        {
            swim.SetSwim();
        }
        else if (_changeTarget is IClimbAction climb)
        {
            climb.UnsetClimb();
        }
    }
}
