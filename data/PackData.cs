using Godot;
using Godot.Collections;

namespace tmfos.data;

/// <summary>
/// ゲーム関連データのセット
/// </summary>
public class PackData : GameDataRoot
{
    public PlayerData PlayerData { get; private set; } = new();
    public ItemData ItemData { get; private set; } = new();
    public StageData StageData { get; private set; } = new();
    public FlagData FlagData { get; private set; } = new();

    private StageData _stageDataBackup = new();

    public void StartNewGame(int scenarioNo = 0)
    {
        PlayerData = new();
        ItemData = new();
        StageData = new()
        {
            ScenarioNo = scenarioNo
        };
        FlagData = new();
        Backup();
    }

    public void Backup()
    {
        _stageDataBackup = StageData.Copy();
    }

    public void Restore()
    {
        StageData = _stageDataBackup.Copy();
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        Error e = PlayerData.SetConfigFile(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = ItemData.SetConfigFile(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = StageData.SetConfigFile(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = FlagData.SetConfigFile(file);
        return e;
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        Error e = PlayerData.GetConfigFile(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = ItemData.GetConfigFile(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = StageData.GetConfigFile(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = FlagData.GetConfigFile(file);
        return e;
    }

    public override Error CheckNecessaryKey(ConfigFile file)
    {
        Error e = PlayerData.CheckNecessaryKey(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = ItemData.CheckNecessaryKey(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = StageData.CheckNecessaryKey(file);

        if (e is not Error.Ok)
        {
            return e;
        }

        e = FlagData.CheckNecessaryKey(file);
        return e;
    }

    public override void RemoveIllegalKey(ConfigFile file)
    {
        PlayerData.RemoveIllegalKey(file);
        ItemData.RemoveIllegalKey(file);
        StageData.RemoveIllegalKey(file);
        FlagData.RemoveIllegalKey(file);
    }

    public override string[] GetSectionKeys(ConfigFile file)
    {
        return [.. PlayerData.GetSectionKeys(file), .. ItemData.GetSectionKeys(file), .. StageData.GetSectionKeys(file), .. FlagData.GetSectionKeys(file)];
    }

    public override Array GetSectionValues(ConfigFile file)
    {
        return [.. PlayerData.GetSectionValues(file), .. ItemData.GetSectionValues(file), .. StageData.GetSectionValues(file), .. FlagData.GetSectionValues(file)];
    }
}
