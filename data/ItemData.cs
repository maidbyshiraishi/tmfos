using Godot;
using Godot.Collections;

namespace tmfos.data;

/// <summary>
/// アイテムデータ
/// </summary>
public class ItemData : GameDataRoot
{
    public static readonly string SectionName = "ItemData";
    public static readonly string ShoesItemKey = "Shoes";
    public static readonly string SwimItemKey = "Swim";
    public static readonly string WallJumpItemKey = "WallJump";
    public static readonly string PenetrationItemKey = "Penetration";
    public static readonly string LampItemKey = "Lamp";
    public static readonly string SearchItemKey = "Search";
    public static readonly string ArmorItemKey = "Armor";
    public static readonly string WeaponItemKey = "Weapon";
    public static readonly Array<string> NecessaryKey = [];
    public static readonly Array<string> AllKey = [ShoesItemKey, SwimItemKey, WallJumpItemKey, PenetrationItemKey, LampItemKey, SearchItemKey, ArmorItemKey, WeaponItemKey];

    public bool Shoes { get; set; }
    public bool Swim { get; set; }
    public bool WallJump { get; set; }
    public bool Penetration { get; set; }
    public bool Lamp { get; set; }
    public bool Search { get; set; }
    public int Armor { get; set; }
    public int Weapon { get; set; }

    public ItemData()
    {
        Shoes = false;
        Swim = false;
        WallJump = false;
        Penetration = false;
        Lamp = false;
        Search = false;
        Armor = 0;
        Weapon = 0;
    }

    public ItemData Copy()
    {
        return new()
        {
            Shoes = Shoes,
            Swim = Swim,
            WallJump = WallJump,
            Penetration = Penetration,
            Lamp = Lamp,
            Search = Search,
            Armor = Armor,
            Weapon = Weapon
        };
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        SetData(file, SectionName, ShoesItemKey, Shoes ? 1 : 0);
        SetData(file, SectionName, SwimItemKey, Swim ? 1 : 0);
        SetData(file, SectionName, WallJumpItemKey, WallJump ? 1 : 0);
        SetData(file, SectionName, PenetrationItemKey, Penetration ? 1 : 0);
        SetData(file, SectionName, LampItemKey, Lamp ? 1 : 0);
        SetData(file, SectionName, SearchItemKey, Search ? 1 : 0);
        SetData(file, SectionName, ArmorItemKey, Armor);
        SetData(file, SectionName, WeaponItemKey, Weapon);
        return Error.Ok;
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        Shoes = GetData(file, SectionName, ShoesItemKey) == 1;
        Swim = GetData(file, SectionName, SwimItemKey) == 1;
        WallJump = GetData(file, SectionName, WallJumpItemKey) == 1;
        Penetration = GetData(file, SectionName, PenetrationItemKey) == 1;
        Lamp = GetData(file, SectionName, LampItemKey) == 1;
        Search = GetData(file, SectionName, SearchItemKey) == 1;
        Armor = GetData(file, SectionName, ArmorItemKey);
        Weapon = GetData(file, SectionName, WeaponItemKey);
        return Error.Ok;
    }

    public override Error CheckNecessaryKey(ConfigFile file)
    {
        foreach (string key in NecessaryKey)
        {
            if (!HasData(file, SectionName, key))
            {
                GD.PrintErr($"ItemDataに必須キー{key}が存在しません。");
                return Error.InvalidData;
            }
        }

        return Error.Ok;
    }

    public override void RemoveIllegalKey(ConfigFile file)
    {
        string[] keys = file.GetSectionKeys(SectionName);

        foreach (string key in keys)
        {
            if (!AllKey.Contains(key))
            {
                GD.PrintErr($"ItemDataには存在しないキー{key}を削除します。");
                RemoveData(file, SectionName, key);
            }
        }
    }

    public override string[] GetSectionKeys(ConfigFile file)
    {
        return [.. AllKey];
    }

    public override Array GetSectionValues(ConfigFile file)
    {
        return [Shoes ? 1 : 0, Swim ? 1 : 0, WallJump ? 1 : 0, Penetration ? 1 : 0, Lamp ? 1 : 0, Search ? 1 : 0, Armor, Weapon];
    }
}
