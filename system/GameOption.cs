using Godot;
using static Godot.DisplayServer;

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
    private static readonly Vector2I DefaultPosition = new(8, 32);

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

        if (!LoadScreenOptions())
        {
            CalcScreenOptions();
        }

        ApplyScreenOptions();
        LoadOptions();
        SetOptions();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            SaveScreenOptions();
            GetTree().Quit();
        }
    }

    public override void _UnhandledInput(InputEvent ievent)
    {
        if (ievent.IsActionPressed("toggle_fullscreen"))
        {
            Fullscreen = !Fullscreen;
            ChangeWindowMode();
        }
    }

    public void ResetOptions()
    {
        BgmVolume = DefaultVolume;
        SeVolume = DefaultVolume;
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
            GD.PrintErr($"設定ファイル{OptionsFilePath}を保存できませんでした。エラーコードは{e}です。");
        }
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
    public void ChangeWindowMode()
    {
        //設定がウィンドウで現在フルスクリーンならウィンドウに切り替える
        if (!Fullscreen && WindowGetMode() is WindowMode.Fullscreen)
        {
            WindowSetMode(WindowMode.Windowed);
            RestoreWindowSizeAndPosition();
        }
        //設定がフルスクリーンで現在ウィンドウならフルスクリーンに切り替える
        else if (Fullscreen && WindowGetMode() is WindowMode.Windowed)
        {
            StoreWindowSizeAndPosition();
            WindowSetMode(WindowMode.Fullscreen);
        }
    }

    public void ChangeOnlyWindowMode()
    {
        //設定がウィンドウで現在フルスクリーンならウィンドウに切り替える
        if (!Fullscreen && WindowGetMode() is WindowMode.Fullscreen)
        {
            WindowSetMode(WindowMode.Windowed);
        }
        //設定がフルスクリーンで現在ウィンドウならフルスクリーンに切り替える
        else if (Fullscreen && WindowGetMode() is WindowMode.Windowed)
        {
            WindowSetMode(WindowMode.Fullscreen);
        }
    }

    public void StoreWindowSizeAndPosition()
    {
        Vector2I size = WindowGetSize();
        Vector2I position = WindowGetPosition();
        _screenOptions.SetValue("ScreenOptions", "window_width_override", size.X);
        _screenOptions.SetValue("ScreenOptions", "window_height_override", size.Y);
        _screenOptions.SetValue("ScreenOptions", "initial_position", position);
    }

    public void RestoreWindowSizeAndPosition()
    {
        int window_width_override = _screenOptions.GetValue("ScreenOptions", "window_width_override", DefaultSizeX).AsInt32();
        int window_height_override = _screenOptions.GetValue("ScreenOptions", "window_height_override", DefaultSizeY).AsInt32();
        Vector2I initial_position = _screenOptions.GetValue("ScreenOptions", "initial_position", DefaultPosition).AsVector2I();
        WindowSetSize(new(window_width_override, window_height_override));
        WindowSetPosition(initial_position);
    }

    public void CalcScreenOptions()
    {
        // ウィンドウモードを強制する
        Fullscreen = false;
        ChangeOnlyWindowMode();
        // 基本的な情報を集める
        Rect2I screenRect = ScreenGetUsableRect();
        Vector2I screenOrigin = screenRect.Position;
        Vector2I screenSize = screenRect.Size;
        Vector2I windowContentOnlySize = WindowGetSize();
        Vector2I windowDecoratedSize = WindowGetSizeWithDecorations();
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
        Vector2I newWindowPosition = new(newWindowPositionX, newWindowPositionY);
        _screenOptions.Clear();
        _screenOptions.SetValue("ScreenOptions", "mode", Fullscreen);
        _screenOptions.SetValue("ScreenOptions", "window_width_override", newWindowSizeX);
        _screenOptions.SetValue("ScreenOptions", "window_height_override", newWindowSizeY);
        _screenOptions.SetValue("ScreenOptions", "initial_position", newWindowPosition);
    }

    public void ApplyScreenOptions()
    {
        Fullscreen = _screenOptions.GetValue("ScreenOptions", "mode", false).AsBool();
        int window_width_override = _screenOptions.GetValue("ScreenOptions", "window_width_override", DefaultSizeX).AsInt32();
        int window_height_override = _screenOptions.GetValue("ScreenOptions", "window_height_override", DefaultSizeY).AsInt32();
        Vector2I windowSize = new(window_width_override, window_height_override);
        Vector2I initial_position = _screenOptions.GetValue("ScreenOptions", "initial_position", DefaultPosition).AsVector2I();
        ChangeOnlyWindowMode();
        WindowSetSize(windowSize);
        WindowSetPosition(initial_position);
    }

    public void SaveScreenOptions()
    {
        Vector2I size = WindowGetSize();
        Vector2I position = WindowGetPosition();
        _screenOptions.SetValue("ScreenOptions", "mode", Fullscreen);

        if (!Fullscreen)
        {
            _screenOptions.SetValue("ScreenOptions", "window_width_override", size.X);
            _screenOptions.SetValue("ScreenOptions", "window_height_override", size.Y);
            _screenOptions.SetValue("ScreenOptions", "initial_position", position);
        }

        Error e = _screenOptions.Save(ScreenOptionsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{ScreenOptionsFilePath}を保存できませんでした。エラーコードは{e}です。");
        }
    }

    public bool LoadScreenOptions()
    {
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

        Fullscreen = _screenOptions.GetValue("ScreenOptions", "mode", false).AsBool();
        int window_width_override = _screenOptions.GetValue("ScreenOptions", "window_width_override", DefaultSizeX).AsInt32();
        int window_height_override = _screenOptions.GetValue("ScreenOptions", "window_height_override", DefaultSizeY).AsInt32();
        Vector2I initial_position = _screenOptions.GetValue("ScreenOptions", "initial_position", DefaultPosition).AsVector2I();
        _screenOptions.SetValue("ScreenOptions", "mode", Fullscreen);
        _screenOptions.SetValue("ScreenOptions", "window_width_override", window_width_override);
        _screenOptions.SetValue("ScreenOptions", "window_height_override", window_height_override);
        _screenOptions.SetValue("ScreenOptions", "initial_position", initial_position);
        return true;
    }
}
