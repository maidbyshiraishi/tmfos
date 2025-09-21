using Godot;
using tmfos.data;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ステータスダイアログを開くコマンド
/// </summary>
public partial class OpenStatusDialogCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        ItemData itemData = GetNode<GameData>("/root/GameData").GetItemData();
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/status_dialog.tscn", "StatusDialog", [itemData.Shoes, itemData.Swim, itemData.WallJump, itemData.Penetration, itemData.Lamp, itemData.Search, itemData.Armor, itemData.Weapon]);
    }
}
