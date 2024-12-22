using Godot;

namespace tmfos.system;

/// <summary>
/// ゲーム設定
/// </summary>
public partial class GameOption : Node
{
    // データ保存先
    private static readonly string OptionFilePath = "user://options.dat";

    private static readonly float DefaultVolume = 50f;

    private ConfigFile _options = new();

    /// <summary>
    /// BGM音量
    /// </summary>
    public float BgmVolume { get; set; } = DefaultVolume;

    /// <summary>
    /// 効果音音量
    /// </summary>
    public float SeVolume { get; set; } = DefaultVolume;

    /// <summary>
    /// ウィンドウ・フルスクリーン表示
    /// </summary>
    public bool Fullscreen { get; set; } = false;

    public override void _Ready()
    {
        LoadOptions();
        SetOptions();
    }

    public void ResetDefault()
    {
        BgmVolume = DefaultVolume;
        SeVolume = DefaultVolume;
        Fullscreen = false;
    }

    /// <summary>
    /// 設定ファイルをまとめて読み込む
    /// </summary>
    public void LoadOptions()
    {
        _options.Clear();
        ResetDefault();
        Error e = _options.Load(OptionFilePath);

        if (e is Error.FileNotFound)
        {
            return;
        }

        if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{OptionFilePath}を読み込めませんでした。エラーコードは{e}です。");
            return;
        }

        string bgm = _options.GetValue("Volume", "BGM", DefaultVolume).ToString();

        if (!string.IsNullOrWhiteSpace(bgm) && float.TryParse(bgm, out float fbgm))
        {
            BgmVolume = Mathf.Clamp(fbgm, 0f, 100f);
        }

        string se = _options.GetValue("Volume", "SE", DefaultVolume).ToString();

        if (!string.IsNullOrWhiteSpace(se) && float.TryParse(se, out float fse))
        {
            SeVolume = Mathf.Clamp(fse, 0f, 100f);
        }

        string fullscreen = _options.GetValue("Window", "Fullscreen", false).ToString();

        if (!string.IsNullOrWhiteSpace(fullscreen) && bool.TryParse(fullscreen, out bool bfullscreen))
        {
            Fullscreen = bfullscreen;
        }
    }

    /// <summary>
    /// ゲームシステム設定を保存する
    /// </summary>
    public void SaveOptions()
    {
        BgmVolume = Mathf.Clamp(BgmVolume, 0f, 100f);
        SeVolume = Mathf.Clamp(SeVolume, 0f, 100f);
        _options.SetValue("Volume", "BGM", BgmVolume);
        _options.SetValue("Volume", "SE", SeVolume);
        _options.SetValue("Window", "Fullscreen", Fullscreen);
        Error e = _options.Save(OptionFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"ゲーム設定の保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
        }
    }

    /// <summary>
    /// ゲームシステム設定を適用する
    /// </summary>
    public void SetOptions()
    {
        SetOptionsVolume();
        SetOptionsFullscreen();
    }

    /// <summary>
    /// BGM音量を適用する
    /// </summary>
    public void SetOptionsVolume()
    {
        BgmVolume = Mathf.Clamp(BgmVolume, 0f, 100f);
        SeVolume = Mathf.Clamp(SeVolume, 0f, 100f);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("BGM"), ToDecibel(BgmVolume));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SE"), ToDecibel(SeVolume));
    }

    /// <summary>
    /// 0-100の音量をデシベルに変換する
    /// </summary>
    /// <param name="volume">音量</param>
    /// <returns>デシベル</returns>
    private static float ToDecibel(float volume)
    {
        float v = Mathf.Clamp(volume / 100f, 0f, 1f);
        float db = Mathf.LinearToDb(v);
        return db;
    }

    /// <summary>
    /// ウィンドウ・フルスクリーン表示を適用する
    /// </summary>
    public void SetOptionsFullscreen()
    {
        //設定がウィンドウで現在フルスクリーンならウィンドウに切り替える
        if (!Fullscreen && DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Fullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            //寸法が設定サイズより小さい場合は補正する
            Vector2 size = DisplayServer.WindowGetSize();
            int width = (int)ProjectSettings.GetSetting("display/window/size/viewport_width");
            int height = (int)ProjectSettings.GetSetting("display/window/size/viewport_height");

            if (size.X < width || size.Y < height)
            {
                DisplayServer.WindowSetSize(new Vector2I(width, height));
            }
        }
        //設定がフルスクリーンで現在ウィンドウならフルスクリーンに切り替える
        else if (Fullscreen && DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Windowed)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }

    public override void _UnhandledInput(InputEvent ievent)
    {
        if (ievent.IsActionPressed("toggle_fullscreen"))
        {
            Fullscreen = !Fullscreen;
            SetOptionsFullscreen();
            SaveOptions();
        }
    }
}
