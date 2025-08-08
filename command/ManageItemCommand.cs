using Godot;
using tmfos.item;
using tmfos.player;

namespace tmfos.command;

/// <summary>
/// アイテムを取得・削除するコマンド
/// </summary>
public partial class ManageItemCommand : CommandNode
{
    /// <summary>
    /// 取得するアイテム
    /// </summary>
    [Export]
    public ItemType Item { get; set; } = ItemType.None;

    /// <summary>
    /// アイテムの所有・未所有
    /// </summary>
    [Export]
    public bool Flag { get; set; } = true;

    public override void DoCommand(Node node, bool flag)
    {
        if (node is Player player)
        {
            player.ManageItem(Item, Flag);
        }
    }
}
