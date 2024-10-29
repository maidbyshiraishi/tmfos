using Godot;
using tmfos.mob;
using tmfos.stage;
using tmfos.system;

namespace tmfos.player;

/// <summary>
/// カメラ
/// </summary>
public partial class Camera : Camera2D, IGameNode
{
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.GameNodeGroup);
        Enabled = false;
    }

    public void InitializeNode()
    {
        // カメラの移動範囲を制限する
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        TileMapLayer map = stageRoot.GetNode<TileMapLayer>("TileMap/Ground");
        Rect2I limits = map.GetUsedRect();
        Vector2I tileSetSize = map.TileSet.TileSize;
        LimitTop = limits.Position.Y * tileSetSize.Y;
        LimitBottom = limits.End.Y * tileSetSize.Y;
        LimitLeft = limits.Position.X * tileSetSize.X;
        LimitRight = limits.End.X * tileSetSize.X;
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;

        // 画面サイズがビューポートよりも小さい場合は拡張する
        if (Mathf.Abs(LimitLeft) + Mathf.Abs(LimitRight) < viewportSize.X)
        {
            // 左右に拡張する
            int expand = (int)(viewportSize.X - (Mathf.Abs(LimitLeft) + Mathf.Abs(LimitRight))) / 2;
            LimitLeft += Mathf.Sign(LimitLeft) * expand;
            LimitRight += Mathf.Sign(LimitRight) * expand;
        }

        if (Mathf.Abs(LimitTop) + Mathf.Abs(LimitBottom) < viewportSize.Y)
        {
            //上方向へ拡張する
            int expand = (int)(viewportSize.Y - (Mathf.Abs(LimitTop) + Mathf.Abs(LimitBottom)));
            LimitTop += Mathf.Sign(LimitTop) * expand;
        }
    }

    public void FinalizeNode()
    {
    }

    public void RemoveNode()
    {
    }
}
