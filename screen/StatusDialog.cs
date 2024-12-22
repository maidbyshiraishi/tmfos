using Godot;
using tmfos.data;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// ステータスダイアログ
/// </summary>
public partial class StatusDialog : DialogRoot
{
    private readonly ItemData _itemData = new();

    protected override void InitializeNode()
    {
        GameData gdata = GetNode<GameData>("/root/GameData");
        GetNode<AnimatedSprite2D>("ItemShoes").Play(_itemData.Shoes ? "closed" : "opened");
        GetNode<AnimatedSprite2D>("ItemSwim").Play(_itemData.Swim ? "closed" : "opened");
        GetNode<AnimatedSprite2D>("ItemWallJump").Play(_itemData.WallJump ? "closed" : "opened");
        GetNode<AnimatedSprite2D>("ItemPenetration").Play(_itemData.Penetration ? "closed" : "opened");
        GetNode<AnimatedSprite2D>("ItemLamp").Play(_itemData.Lamp ? "closed" : "opened");
        GetNode<AnimatedSprite2D>("ItemSearch").Play(_itemData.Search ? "closed" : "opened");
        int boss1 = gdata.GetFlagData("boss_0001");
        GetNode<AnimatedSprite2D>("ItemBoss1").Play(boss1 == 0 ? "opened" : "closed");
        int boss2 = gdata.GetFlagData("boss_0002");
        GetNode<AnimatedSprite2D>("ItemBoss2").Play(boss2 == 0 ? "opened" : "closed");
        int boss3 = gdata.GetFlagData("boss_0003");
        GetNode<AnimatedSprite2D>("ItemBoss3").Play(boss3 == 0 ? "opened" : "closed");
        GetNode<AnimatedSprite2D>("ItemArmor").Play(_itemData.Armor > 0 ? "closed" : "opened");
        GetNode<AnimatedSprite2D>("ItemWeapon").Play(_itemData.Weapon > 0 ? "closed" : "opened");

        if (_itemData.Armor == 0)
        {
            GetNode<Label>("ItemArmorLabel").Hide();
        }
        else
        {
            GetNode<Label>("ItemArmorLabel").Text = _itemData.Armor.ToString();
        }

        if (_itemData.Weapon == 0)
        {
            GetNode<Label>("ItemWeaponLabel").Hide();
        }
        else
        {
            GetNode<Label>("ItemWeaponLabel").Text = _itemData.Weapon.ToString();
        }
    }

    public override void GetArgument()
    {
        GetGameArgument("StatusDialog");

        if (
            m_argument is not null &&
            m_argument.Length == 8 &&
            m_argument[0].VariantType is Variant.Type.Bool &&
            m_argument[1].VariantType is Variant.Type.Bool &&
            m_argument[2].VariantType is Variant.Type.Bool &&
            m_argument[3].VariantType is Variant.Type.Bool &&
            m_argument[4].VariantType is Variant.Type.Bool &&
            m_argument[5].VariantType is Variant.Type.Bool &&
            m_argument[6].VariantType is Variant.Type.Int &&
            m_argument[7].VariantType is Variant.Type.Int
        )
        {
            _itemData.Shoes = m_argument[0].AsBool();
            _itemData.Swim = m_argument[1].AsBool();
            _itemData.WallJump = m_argument[2].AsBool();
            _itemData.Penetration = m_argument[3].AsBool();
            _itemData.Lamp = m_argument[4].AsBool();
            _itemData.Search = m_argument[5].AsBool();
            _itemData.Armor = m_argument[6].AsInt32();
            _itemData.Weapon = m_argument[7].AsInt32();
        }
    }
}
