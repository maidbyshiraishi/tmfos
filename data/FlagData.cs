using Godot;
using Godot.Collections;

namespace tmfos.data;

/// <summary>
/// ゲームフラグ
/// </summary>
public class FlagData : GameDataRoot
{
    public static readonly string SectionName = "FlagData";
    public static readonly Array<string> NecessaryKey = [];
    public static readonly Array<string> AllKey = [];
    private readonly Dictionary<string, int> _flag = [];

    public FlagData()
    {
    }

    public FlagData(Dictionary<string, int> flag)
    {
        foreach (string key in flag.Keys)
        {
            flag.Add(key, flag[key]);
        }
    }

    public FlagData Copy()
    {
        return new(_flag);
    }

    public void SetFlag(string key, int value)
    {
        _flag[key] = value;
    }

    public void AddFlag(string key, int value)
    {
        SetFlag(key, GetFlag(key) + value);
    }

    public int GetFlag(string key)
    {
        return _flag.TryGetValue(key, out int value) ? value : 0;
    }

    public void RemoveFlag(string key)
    {
        if (!_flag.ContainsKey(key))
        {
            return;
        }

        _ = _flag.Remove(key);
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        foreach (string key in _flag.Keys)
        {
            file.SetValue(SectionName, key, _flag[key]);
        }

        return Error.Ok;
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        if (!file.HasSection(SectionName))
        {
            return Error.Ok;
        }

        string[] keys = file.GetSectionKeys(SectionName);

        foreach (string key in keys)
        {
            Variant v = file.GetValue(SectionName, key);

            if (v.VariantType is Variant.Type.Int)
            {
                _flag.Add(key, v.AsInt32());
            }
        }

        return Error.Ok;
    }

    public override string[] GetSectionKeys(ConfigFile file)
    {
        return [.. _flag.Keys];
    }

    public override Array GetSectionValues(ConfigFile file)
    {
        return [.. _flag.Values];
    }
}
