using Godot;
using Godot.Collections;

namespace tmfos.data;

/// <summary>
/// ステージデータ
/// </summary>
public class StageData : GameDataRoot
{
    public static readonly string SectionName = "StageData";
    public static readonly string StageNoKey = "StageNo";
    public static readonly string DoorNoKey = "DoorNo";
    public static readonly string ScenarioNoKey = "ScenarioNo";
    public static readonly string TimeKey = "Time";
    public static readonly Array<string> NecessaryKey = [StageNoKey, DoorNoKey, ScenarioNoKey];
    public static readonly Array<string> AllKey = [StageNoKey, DoorNoKey, ScenarioNoKey, TimeKey];

    public int StageNo { get; set; } = 1;
    public int DoorNo { get; set; } = 1;
    public int ScenarioNo { get; set; } = 0;
    public int TakeOverStageNo { get; set; } = 0;
    public double TakeOverPlayerLifeTime { get; set; } = 0;

    public StageData Copy()
    {
        return new()
        {
            StageNo = StageNo,
            DoorNo = DoorNo,
            ScenarioNo = ScenarioNo,
            TakeOverStageNo = TakeOverStageNo,
            TakeOverPlayerLifeTime = TakeOverPlayerLifeTime
        };
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        StageNo = GetData(file, SectionName, StageNoKey);
        DoorNo = GetData(file, SectionName, DoorNoKey);
        ScenarioNo = GetData(file, SectionName, ScenarioNoKey);
        TakeOverStageNo = 0;
        TakeOverPlayerLifeTime = 0;
        return Error.Ok;
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        SetData(file, SectionName, StageNoKey, StageNo);
        SetData(file, SectionName, DoorNoKey, DoorNo);
        SetData(file, SectionName, ScenarioNoKey, ScenarioNo);
        return Error.Ok;
    }

    public override Error CheckNecessaryKey(ConfigFile file)
    {
        foreach (string key in NecessaryKey)
        {
            if (!HasData(file, SectionName, key))
            {
                GD.PrintErr($"StageDataに必須キー{key}が存在しません。");
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
                GD.PrintErr($"StageDataには存在しないキー{key}を削除します。");
                RemoveData(file, SectionName, key);
            }
        }
    }

    public override string[] GetSectionKeys(ConfigFile file)
    {
        return [.. NecessaryKey];
    }

    public override Array GetSectionValues(ConfigFile file)
    {
        return [StageNo, DoorNo, ScenarioNo];
    }
}
