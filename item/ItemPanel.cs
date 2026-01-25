using Godot;
using Godot.Collections;
using tmfos.command.decoration;
using tmfos.command.stage;
using tmfos.trigger;

namespace tmfos.item;

/// <summary>
/// アイテムの親
/// </summary>
public partial class ItemPanel : TriggerArea2D
{
    [Export]
    public ItemType Item { get; set; } = ItemType.None;

    private Dictionary names = new() {
        {(int)ItemType.Shoes, "靴"},
        {(int)ItemType.Swim, "水泳"},
        {(int)ItemType.WallJump, "壁ジャンプ"},
        {(int)ItemType.Penetration, "貫通弾"},
        {(int)ItemType.Lamp, "ランプ"},
        {(int)ItemType.Search, "名探偵"},
        {(int)ItemType.Armor, "防御力アップ"},
        {(int)ItemType.Weapon, "攻撃力アップ"}
    };

    private Dictionary resources = new() {
        {(int)ItemType.Shoes, "res://contents/animation/item/item_shoes.tres"},
        {(int)ItemType.Swim, "res://contents/animation/item/item_swim.tres"},
        {(int)ItemType.WallJump, "res://contents/animation/item/item_wall_jump.tres"},
        {(int)ItemType.Penetration, "res://contents/animation/item/item_penetration.tres"},
        {(int)ItemType.Lamp, "res://contents/animation/item/item_lamp.tres"},
        {(int)ItemType.Search, "res://contents/animation/item/item_search.tres"},
        {(int)ItemType.Armor, "res://contents/animation/item/item_armor.tres"},
        {(int)ItemType.Weapon, "res://contents/animation/item/item_weapon.tres"}
    };

    public override void _Ready()
    {
        base._Ready();

        if (Item is ItemType.None)
        {
            return;
        }

        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite2D.SpriteFrames = GD.Load<SpriteFrames>(resources[(int)Item].AsString());
        SwitchAnimation();
        ManageItemCommand manageItemCommand = GetNode<ManageItemCommand>("ManageItemCommand");
        manageItemCommand.Item = Item;
        ShowFloatingMessageCommand showFloatingMessageCommand = GetNode<ShowFloatingMessageCommand>("ShowFloatingMessageCommand");
        showFloatingMessageCommand.Message = names[(int)Item].AsString();
    }

    public override void SetOpened(bool opened)
    {
        base.SetOpened(opened);
        SwitchAnimation();
    }

    private void SwitchAnimation()
    {
        SwitchOpenOrCloseAnimationCommand switchOpenOrCloseAnimationCommand = GetNode<SwitchOpenOrCloseAnimationCommand>("SwitchOpenOrCloseAnimationCommand");
        switchOpenOrCloseAnimationCommand.Opened = m_opened;
    }
}
