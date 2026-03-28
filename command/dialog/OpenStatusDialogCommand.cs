using Godot;
using tmfos.data;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ステータスダイアログを開くコマンド
/// </summary>
public partial class OpenStatusDialogCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        ItemData itemData = GetNode<GameDataManager>("/root/GameDataManager").GetItemData();
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/status_dialog.tscn", "StatusDialog", [itemData.Shoes, itemData.Swim, itemData.WallJump, itemData.Penetration, itemData.Lamp, itemData.Search, itemData.Armor, itemData.Weapon]);
    }
}
