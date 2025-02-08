using Godot;

namespace tmfos.system;

/// <summary>
/// ゲーム設定
/// </summary>
public partial class GameOption : Node
{
    private static readonly string OptionsFilePath = "user://options.dat";
    private static readonly float DefaultVolume = 50f;

    private static readonly string ScreenOptionsFilePath = "user://screen_options.dat";
    private static readonly int DefaultSizeX = 640;
    private static readonly int DefaultSizeY = 480;

    private static readonly string ProjectSettingsFilePath = "user://override.cfg";

    private ConfigFile _audioVolumeOptions = new();
    private ConfigFile _screenOptions = new();

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

#if TOOLS
        // デバッグ実行時はAbsolute指定は動作しないため、毎回位置調整する。
        _ = CalcScreenOptions();
#else
        if (!LoadScreenOptions())
        {
            _ = CalcScreenOptions();
            //OS.SetRestartOnExit(true);
            //GetTree().Quit();
        }
#endif

        LoadOptions();
        SetOptions();
    }

    public void ResetOptions()
    {
        BgmVolume = DefaultVolume;
        SeVolume = DefaultVolume;
    }

    /// <summary>
    /// 設定ファイルをまとめて読み込む
    /// </summary>
    public void LoadOptions()
    {
        _audioVolumeOptions.Clear();
        ResetOptions();
        Error e = _audioVolumeOptions.Load(OptionsFilePath);

        if (e is Error.FileNotFound)
        {
            return;
        }
        else if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{OptionsFilePath}を読み込めませんでした。エラーコードは{e}です。");
            return;
        }

        BgmVolume = (float)Mathf.Clamp(_audioVolumeOptions.GetValue("Volume", "BGM", DefaultVolume).AsDouble(), 0f, 100f);
        SeVolume = (float)Mathf.Clamp(_audioVolumeOptions.GetValue("Volume", "SE", DefaultVolume).AsDouble(), 0f, 100f);
    }

    /// <summary>
    /// ゲームシステム設定を保存する
    /// </summary>
    public void SaveOptions()
    {
        BgmVolume = Mathf.Clamp(BgmVolume, 0f, 100f);
        SeVolume = Mathf.Clamp(SeVolume, 0f, 100f);
        _audioVolumeOptions.SetValue("Volume", "BGM", BgmVolume);
        _audioVolumeOptions.SetValue("Volume", "SE", SeVolume);
        Error e = _audioVolumeOptions.Save(OptionsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{OptionsFilePath}を保存できません弟子t。エラーコードは{e}です。");
        }
    }

    /// <summary>
    /// BGM音量を適用する
    /// </summary>
    public void SetOptions()
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
    public void SetWindowMode()
    {
        //設定がウィンドウで現在フルスクリーンならウィンドウに切り替える
        if (!Fullscreen && DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Fullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            LoadWindowSizeAndPosition();
        }
        //設定がフルスクリーンで現在ウィンドウならフルスクリーンに切り替える
        else if (Fullscreen && DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Windowed)
        {
            SaveWindowSizeAndPosition();
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }

    public void ChangeWindowMode()
    {
        //設定がウィンドウで現在フルスクリーンならウィンドウに切り替える
        if (!Fullscreen && DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Fullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
        //設定がフルスクリーンで現在ウィンドウならフルスクリーンに切り替える
        else if (Fullscreen && DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Windowed)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }

    public void LoadWindowSizeAndPosition()
    {
        int window_width_override = ProjectSettings.GetSetting("display/window/size/window_width_override", DefaultSizeX).AsInt32();
        int window_height_override = ProjectSettings.GetSetting("display/window/size/window_height_override", DefaultSizeY).AsInt32();
        Vector2I initial_position = ProjectSettings.GetSetting("display/window/size/initial_position").AsVector2I();
        DisplayServer.WindowSetSize(new(window_width_override, window_height_override));
        DisplayServer.WindowSetPosition(initial_position);
    }

    public void SaveWindowSizeAndPosition()
    {
        Vector2I size = DisplayServer.WindowGetSize();
        Vector2I position = DisplayServer.WindowGetPosition();

        if (DefaultSizeX == size.X)
        {
            ProjectSettings.Clear("display/window/size/window_width_override");
        }
        else
        {
            ProjectSettings.SetSetting("display/window/size/window_width_override", size.X);
        }

        if (DefaultSizeY == size.Y)
        {
            ProjectSettings.Clear("display/window/size/window_height_override");
        }
        else
        {
            ProjectSettings.SetSetting("display/window/size/window_height_override", size.Y);
        }

        ProjectSettings.SetSetting("display/window/size/initial_position_type", (int)Window.WindowInitialPosition.Absolute);

        if (position == Vector2I.Zero)
        {
            ProjectSettings.Clear("display/window/size/initial_position");
        }
        else
        {
            ProjectSettings.SetSetting("display/window/size/initial_position", position);
        }

        Error e = ProjectSettings.SaveCustom(ProjectSettingsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"プロジェクト設定を保存できませんでした。ゲームは続行されます。エラーの値は{e}です。");
        }
    }

    public override void _UnhandledInput(InputEvent ievent)
    {
        if (ievent.IsActionPressed("toggle_fullscreen"))
        {
            Fullscreen = !Fullscreen;
            SetWindowMode();
        }
    }

    public bool LoadScreenOptions()
    {
        // ウィンドウモードだけ先に済ませる
        Fullscreen = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
        ChangeWindowMode();

        _screenOptions.Clear();
        Error e = _screenOptions.Load(ScreenOptionsFilePath);

        if (e is Error.FileNotFound)
        {
            return false;
        }
        else if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{ScreenOptionsFilePath}を読み込めませんでした。エラーコードは{e}です。");
            return false;
        }

        return _screenOptions.GetValue("ScreenOptions", "Configured", false).AsBool();
    }

    private bool CalcScreenOptions()
    {
        // ウィンドウモードだけ先に済ませる
        Fullscreen = false;
        ChangeWindowMode();

        // 基本的な情報を集める
        Rect2I screenRect = DisplayServer.ScreenGetUsableRect();
        Vector2I screenOrigin = screenRect.Position;
        Vector2I screenSize = screenRect.Size;
        Vector2I windowContentOnlySize = DisplayServer.WindowGetSize();
        Vector2I windowDecoratedSize = DisplayServer.WindowGetSizeWithDecorations();
        // タイトルバーとボーダーのサイズを求める
        Vector2I windowBorderSize = windowDecoratedSize - windowContentOnlySize;
        // 基準サイズは640x480にタイトルバーとボーダーのサイズを加えたもの
        Vector2I windowContentOnlyOriginalSize = new(DefaultSizeX, DefaultSizeY);
        Vector2I windowDecoratedOriginalSize = windowContentOnlyOriginalSize + windowBorderSize;
        // windowDecoratedSizeからscreenSizeに収まるちょうどいい大きさを求める
        // 画面サイズギリギリになり過ぎないように0.8倍する。
        float scaleX = screenSize.X / (float)windowDecoratedOriginalSize.X * 0.8f;
        float scaleY = screenSize.Y / (float)windowDecoratedOriginalSize.Y * 0.8f;
        // 拡大率の低いほうを採用するが、
        float scale = Mathf.Min(scaleX, scaleY);
        int newWindowSizeX = Mathf.FloorToInt(windowContentOnlyOriginalSize.X * scale);
        int newWindowSizeY = Mathf.FloorToInt(windowContentOnlyOriginalSize.Y * scale);

        // screenOriginを基準にウィンドウ位置を求める
        int newWindowPositionX = screenOrigin.X + ((screenSize.X - newWindowSizeX) / 2);
        int newWindowPositionY = screenOrigin.Y + ((screenSize.Y - newWindowSizeY) / 2);

        // 設定を反映する
        DisplayServer.WindowSetSize(new(newWindowSizeX, newWindowSizeY));
        DisplayServer.WindowSetPosition(new(newWindowPositionX, newWindowPositionY));

        _screenOptions.Clear();

#if TOOLS
        // デバッグ実行時は画面設定を未設定にする。
        // デバッグとリリース実行を行き来するとき、falseしておくと再計算が動く。
        _screenOptions.SetValue("ScreenOptions", "Configured", false);
#else
        _screenOptions.SetValue("ScreenOptions", "Configured", true);
#endif

        Error e1 = _screenOptions.Save(ScreenOptionsFilePath);

        if (e1 is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{ScreenOptionsFilePath}を保存できません弟子t。エラーコードは{e1}です。");
            return false;
        }

        return true;
    }

    public void SaveScreenOptions()
    {
        if (Fullscreen)
        {
            ProjectSettings.SetSetting("display/window/size/mode", (int)DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            ProjectSettings.Clear("display/window/size/mode");

            // ウィンドウ状態、位置、サイズを保存する
            Vector2I size = DisplayServer.WindowGetSize();
            Vector2I position = DisplayServer.WindowGetPosition();

            if (DefaultSizeX == size.X)
            {
                ProjectSettings.Clear("display/window/size/window_width_override");
            }
            else
            {
                ProjectSettings.SetSetting("display/window/size/window_width_override", size.X);
            }

            if (DefaultSizeY == size.Y)
            {
                ProjectSettings.Clear("display/window/size/window_height_override");
            }
            else
            {
                ProjectSettings.SetSetting("display/window/size/window_height_override", size.Y);
            }

            ProjectSettings.SetSetting("display/window/size/initial_position_type", (int)Window.WindowInitialPosition.Absolute);

            if (position == Vector2I.Zero)
            {
                ProjectSettings.Clear("display/window/size/initial_position");
            }
            else
            {
                ProjectSettings.SetSetting("display/window/size/initial_position", position);
            }
        }

#if TOOLS
        // デバッグ実行時は位置、サイズをクリアしておく。
        // リリース実行後にデバッグ実行した場合もクリアする。
        ProjectSettings.Clear("display/window/size/window_width_override");
        ProjectSettings.Clear("display/window/size/window_height_override");
        ProjectSettings.Clear("display/window/size/initial_position_type");
        ProjectSettings.Clear("display/window/size/initial_position");
#endif

        Error e = ProjectSettings.SaveCustom(ProjectSettingsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"プロジェクト設定を保存できませんでした。ゲームは続行されます。エラーの値は{e}です。");
        }
    }

    public void SaveWindowMode()
    {
        if (Fullscreen)
        {
            ProjectSettings.SetSetting("display/window/size/mode", (int)DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            ProjectSettings.Clear("display/window/size/mode");
        }

        Error e = ProjectSettings.SaveCustom(ProjectSettingsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"プロジェクト設定を保存できませんでした。ゲームは続行されます。エラーの値は{e}です。");
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            SaveScreenOptions();
            GetTree().Quit();
        }
    }
}
