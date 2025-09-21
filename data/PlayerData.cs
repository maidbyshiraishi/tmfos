using Godot;
using Godot.Collections;

namespace tmfos.data;

/// <summary>
/// プレーヤーデータ
/// </summary>
public class PlayerData : GameDataRoot
{
    public static readonly string SectionName = "PlayerData";
    public static readonly string RemainKey = "Remain";
    public static readonly string LifeKey = "Life";
    public static readonly string ScoreKey = "Score";
    public static readonly string ExtendScoreKey = "ExtendScore";
    public static readonly Array<string> NecessaryKey = [RemainKey, LifeKey, ScoreKey, ExtendScoreKey];
    public static readonly Array<string> AllKey = [RemainKey, LifeKey, ScoreKey, ExtendScoreKey];
    public static readonly int InitialLife = 60;
    public static readonly int InitialRemain = 3;
    public static readonly int ExtendScoreThreshold = 50000;

    public int Remain { get; set; }
    public int Life { get; set; }
    public int Score { get; set; }
    public int ExtendScore { get; set; }

    public PlayerData()
    {
        Remain = InitialRemain;
        Life = InitialLife;
        Score = 0;
        ExtendScore = 0;
    }

    public PlayerData Copy()
    {
        return new()
        {
            Remain = Remain,
            Life = Life,
            Score = Score,
            ExtendScore = ExtendScore,
        };
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        SetData(file, SectionName, RemainKey, Remain);
        SetData(file, SectionName, LifeKey, Life);
        SetData(file, SectionName, ScoreKey, Score);
        SetData(file, SectionName, ExtendScoreKey, ExtendScore);
        return Error.Ok;
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        Remain = GetData(file, SectionName, RemainKey);
        Life = GetData(file, SectionName, LifeKey);
        Score = GetData(file, SectionName, ScoreKey);
        ExtendScore = GetData(file, SectionName, ExtendScoreKey);
        return Error.Ok;
    }

    public override Error CheckNecessaryKey(ConfigFile file)
    {
        foreach (string key in NecessaryKey)
        {
            if (!HasData(file, SectionName, key))
            {
                GD.PrintErr($"PlayerDataに必須キー{key}が存在しません。");
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
                GD.PrintErr($"PlayerDataには存在しないキー{key}を削除します。");
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
        return [Remain, Life, Score, ExtendScore];
    }
}
