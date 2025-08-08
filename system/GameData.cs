using Godot;
using System;
using tmfos.data;

namespace tmfos.system;

/// <summary>
/// ゲームデータ管理
/// </summary>
public partial class GameData : Node
{
    public const int DefaultSlotNo = 0;

    /// <summary>
    /// 保存時に暗号化
    /// </summary>
    [Export]
    public bool UseEncriptData { get; set; } = false;

    /// <summary>
    /// 利用するゲームデータ数
    /// </summary>
    public static readonly int NumOfSaveFiles = 6;

    /// <summary>
    /// サムネイルファイルのパス
    /// </summary>
    public static readonly string DataThumbnailPath = "user://save{0:0000}.png";

    public int LastSlotNo { get; private set; } = DefaultSlotNo;

    private static readonly string DataFilePath = "user://save{0:0000}.dat";
    private static readonly string Pass = "password";
    private static readonly string StageSection = "{0:D4}";

    private Image _thumbnail;
    private ConfigFile _file = new();
    private readonly PackData _packData = new();

    /// <summary>
    /// 新しいゲームセッションを開始する
    /// </summary>
    /// <param name="scenarioNo">シナリオ番号</param>
    public void StartNewGame(int scenarioNo = 0)
    {
        _file = new();
        LastSlotNo = DefaultSlotNo;
        _packData.StartNewGame(scenarioNo);
    }

    public void Backup()
    {
        _packData.Backup();
    }

    public void Restore()
    {
        _packData.Restore();
    }

    public PlayerData GetPlayerData()
    {
        return _packData.PlayerData;
    }

    public ItemData GetItemData()
    {
        return _packData.ItemData;
    }

    public StageData GetStageData()
    {
        return _packData.StageData;
    }

    public FlagData GetFlagData()
    {
        return _packData.FlagData;
    }

    /// <summary>
    /// ゲームデータをロードする。
    /// 番号を省略した場合は前回番号を使用する
    /// </summary>
    /// <param name="slotNo">番号</param>
    /// <returns>Error</returns>
    public Error Load(int slotNo = DefaultSlotNo)
    {
        if (slotNo == DefaultSlotNo)
        {
            // 番号が省略かつ前回番号が未使用ならエラー
            if (HasLastSlotNo())
            {
                return Error.FileNotFound;
            }

            // 番号が省略された場合は前回番号を使用する
            slotNo = LastSlotNo;
        }

        StartNewGame();
        string file = string.Format(DataFilePath, slotNo);
        Error e = UseEncriptData ? _file.LoadEncryptedPass(file, Pass) : _file.Load(file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"ロードに失敗しました。エラーの発生したデータはゲームデータ{slotNo}です。ファイル名は{file}です。エラーの値は{e}です。");
            return e;
        }

        e = _packData.CheckNecessaryKey(_file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"CheckNecessaryKey()のエラーにより、ロードに失敗しました。エラーの発生したデータはゲームデータ{slotNo}です。エラーの値は{e}です。");
        }

        _packData.RemoveIllegalKey(_file);
        e = _packData.GetConfigFile(_file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"GetConfigFile()のエラーにより、ロードに失敗しました。エラーの発生したデータはゲームデータ{slotNo}です。エラーの値は{e}です。");
            return e;
        }

        Backup();
        LastSlotNo = slotNo;
        return Error.Ok;
    }

    public Error LoadInitialStartData(int scenarioNo)
    {
        StartNewGame(scenarioNo);
        return Error.Ok;
    }

    /// <summary>
    /// ゲームデータをセーブする
    /// 番号を省略した場合は前回番号を使用する
    /// </summary>
    /// <param name="slotNo">番号</param>
    /// <returns>Error</returns>
    public Error Save(int slotNo = DefaultSlotNo)
    {
        // 番号が省略かつ前回番号が未使用ならエラー
        if (slotNo == DefaultSlotNo && HasLastSlotNo())
        {
            return Error.FileNotFound;
        }
        else if (slotNo == DefaultSlotNo)
        {
            // 番号が省略された場合は前回番号を使用する
            slotNo = LastSlotNo;
        }

        Error e = _packData.SetConfigFile(_file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"SetConfigFile()のエラーにより、セーブに失敗しました。エラーの発生したデータはゲームデータ{slotNo}です。エラーの値は{e}です。");
            return e;
        }

        e = _packData.CheckNecessaryKey(_file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"CheckNecessaryKey()のエラーにより、セーブに失敗しました。エラーの発生したデータはゲームデータ{slotNo}です。エラーの値は{e}です。");
            return e;
        }

        _packData.RemoveIllegalKey(_file);
        string file = string.Format(DataFilePath, slotNo);
        e = UseEncriptData ? _file.LoadEncryptedPass(file, Pass) : _file.Save(file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"セーブに失敗しました。エラーの発生したデータはゲームデータ{slotNo}です。ファイル名は{file}です。エラーの値は{e}です。");
            return e;
        }

        if (_thumbnail is null)
        {
            GD.PrintErr($"サムネイルが生成されていません。サムネイルは保存されません。ゲームは続行されます。");
        }
        else
        {
            string fileThumbnail = string.Format(DataThumbnailPath, slotNo);
            e = _thumbnail.SavePng(fileThumbnail);

            if (e is not Error.Ok)
            {
                GD.PrintErr($"サムネイルの保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
            }
        }

        LastSlotNo = slotNo;
        return Error.Ok;
    }

    /// <summary>
    /// ゲームデータの保存日一覧を取得する
    /// </summary>
    /// <returns>保存日一覧</returns>
    public string[] GetFileDates()
    {
        string[] ret = new string[NumOfSaveFiles];

        for (int i = 1; i <= NumOfSaveFiles; i++)
        {
            ret[i - 1] = GetFileDate(i);
        }

        return ret;
    }

    public string GetFileDate(int slotNo)
    {
        if (slotNo < 1 || NumOfSaveFiles < slotNo)
        {
            return null;
        }

        ConfigFile data = new();
        string file = string.Format(DataFilePath, slotNo);

        if (FileAccess.FileExists(file))
        {
            Error e = UseEncriptData ? data.LoadEncryptedPass(file, Pass) : data.Load(file);

            if (e is not Error.Ok)
            {
                return null;
            }

            long datetime = (long)FileAccess.GetModifiedTime(file);
            string localtime = DateTime.Parse(Time.GetDatetimeStringFromUnixTime(datetime)).ToLocalTime().ToString();
            return localtime;
        }

        return null;
    }

    public void SetStageData(int stageNo, string key, int value)
    {
        _file.SetValue(GetStageSectionName(stageNo), key, value);
    }

    public void AddStageData(int stageNo, string key, int value)
    {
        SetStageData(stageNo, key, GetStageData(stageNo, key) + value);
    }

    /// <summary>
    /// ステージデータを取得する
    /// </summary>
    /// <param name="stageNo">ステージ番号</param>
    /// <param name="key">key</param>
    /// <returns>value</returns>
    public int GetStageData(int stageNo, string key)
    {
        Variant v = _file.GetValue(GetStageSectionName(stageNo), key);
        return v.VariantType is Variant.Type.Int ? v.AsInt32() : 0;
    }

    public bool HasStageData(int stageNo, string key)
    {
        return _file.HasSectionKey(GetStageSectionName(stageNo), key);
    }

    /// <summary>
    /// ステージデータを削除する
    /// </summary>
    /// <param name="stageNo">ステージ番号</param>
    /// <param name="key">key</param>
    public void RemoveStageData(int stageNo, string key)
    {
        _file.EraseSectionKey(GetStageSectionName(stageNo), key);
    }

    /// <summary>
    /// ステージデータをすべて消去する
    /// </summary>
    /// <param name="stageNo">ステージ番号</param>
    public void ClearStageData(int stageNo)
    {
        _file.EraseSection(GetStageSectionName(stageNo));
    }

    private string GetStageSectionName(int stageNo)
    {
        return string.Format(StageSection, stageNo);
    }

    public void GetKeysAndValues(out string[] keys, out Godot.Collections.Array values)
    {
        keys = _packData.GetSectionKeys(_file);
        values = _packData.GetSectionValues(_file);
    }

    public void AddFlagData(string key, int value)
    {
        _packData.FlagData.AddFlag(key, value);
    }

    public void RemoveFlagData(string key)
    {
        _packData.FlagData.RemoveFlag(key);
    }

    public void SetFlagData(string key, int value)
    {
        _packData.FlagData.SetFlag(key, value);
    }

    public int GetFlagData(string key)
    {
        return _packData.FlagData.GetFlag(key);
    }

    public void SetThumbnail(Image imagge)
    {
        _thumbnail = imagge;
    }

    public bool HasLastSlotNo()
    {
        return LastSlotNo != DefaultSlotNo;
    }
}
