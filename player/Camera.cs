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
        if (Mathf.Abs(LimitLeft - LimitRight) < viewportSize.X)
        {
            // 左右に拡張する
            int expand = (int)(viewportSize.X - Mathf.Abs(LimitLeft - LimitRight)) / 2;
            LimitLeft -= expand;
            LimitRight += expand;
        }

        if (Mathf.Abs(LimitTop - LimitBottom) < viewportSize.Y)
        {
            //上方向へ拡張する
            int expand = (int)(viewportSize.Y - Mathf.Abs(LimitTop - LimitBottom));
            LimitTop -= expand;
        }
    }

    public void FinalizeNode()
    {
    }

    public void RemoveNode()
    {
    }
}
