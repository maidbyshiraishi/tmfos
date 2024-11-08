using Godot;
using tmfos.mob;
using tmfos.player;
using tmfos.stage;
using tmfos.trigger;

namespace tmfos.command;

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

    /// <summary>
    /// 開始時に停止するトリガー
    /// </summary>
    [Export]
    public TriggerArea2D CommandTrigger { get; set; }

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.StageEntryPointGroup);
    }

    /// <summary>
    /// プレーヤーの出現位置として設定する
    /// </summary>
    /// <param name="player">Player</param>
    public void SetPlayerStartPosition(Player player)
    {
        player.Direction = Direction;
        player.Position = GlobalPosition;

        if (CommandTrigger is not null)
        {
            CommandTrigger.Disable = true;
        }
    }

    public void EnableCollidableArea(Area2D node)
    {
        if (node is EventFinder finder && finder.EventNode2D is Player)
        {
            CommandTrigger.Disable = false;
        }
    }
}
