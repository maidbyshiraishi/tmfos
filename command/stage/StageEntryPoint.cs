using Godot;
using maid_by_shiraishi.door;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.player;
using maid_by_shiraishi.stage;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// ゲームスタート位置
/// コマンドとしては何もしない
/// </summary>
public partial class StageEntryPoint : Node2D
{
    /// <summary>
    /// ドア番号
    /// </summary>
    [Export]
    public int DoorNo { get; set; } = 0;

    /// <summary>
    /// 出現時プレーヤー方向
    /// </summary>
    [Export]
    public DirectionType Direction { get; set; } = DirectionType.Left;

    public override void _Ready() => AddToGroup(GameStageRoot.StageEntryPointGroup);

    /// <summary>
    /// プレーヤーの出現位置として設定する
    /// </summary>
    /// <param name="player">Player</param>
    public void SetPlayerStartPosition(Player player)
    {
        player.Direction = Direction;
        player.Position = GlobalPosition;
        player.ChangeSprite("walk", Direction);

        if (GetParent() is Warp warp)
        {
            warp.DisableWarp();
        }
    }
}
