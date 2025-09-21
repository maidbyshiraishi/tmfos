using Godot;
using Godot.Collections;

namespace tmfos.data;

/// <summary>
/// ゲームデータ
/// </summary>
public class GameDataRoot
{
    public virtual Error SetConfigFile(ConfigFile file)
    {
        return Error.Ok;
    }

    public virtual Error GetConfigFile(ConfigFile file)
    {
        return Error.Ok;
    }

    public virtual Error CheckNecessaryKey(ConfigFile file)
    {
        return Error.Ok;
    }

    public virtual void RemoveIllegalKey(ConfigFile file)
    {
    }

    public virtual string[] GetSectionKeys(ConfigFile file)
    {
        return [];
    }

    public virtual Array GetSectionValues(ConfigFile file)
    {
        return [];
    }

    public void SetData(ConfigFile file, string sectionName, string key, int value)
    {
        file.SetValue(sectionName, key, value);
    }

    public int GetData(ConfigFile file, string sectionName, string key)
    {
        if (HasData(file, sectionName, key))
        {
            Variant data = file.GetValue(sectionName, key);
            return data.VariantType is Variant.Type.Int ? (int)data : 0;
        }

        return 0;
    }

    public bool HasData(ConfigFile file, string sectionName, string key)
    {
        return file.HasSection(sectionName) && file.HasSectionKey(sectionName, key);
    }

    public void RemoveData(ConfigFile file, string sectionName, string key)
    {
        if (HasData(file, sectionName, key))
        {
            file.EraseSectionKey(sectionName, key);
        }
    }

    public void ClearSection(ConfigFile file, string sectionName)
    {
        if (file.HasSection(sectionName))
        {
            file.EraseSection(sectionName);
        }
    }
}
