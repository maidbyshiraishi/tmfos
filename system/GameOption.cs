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
        CorrectOnScreen();
        LoadOptions();
        SetOptions();
        GetNode<Timer>("Timer").Start();
    }

    public override void _Notification(int what)
    {
        // ゲーム終了時にウィンドウの位置とサイズを保存する
        if (what == NotificationWMCloseRequest)
        {
            SaveScreenOptions();
            GetTree().Quit();
        }
    }

    public override void _UnhandledInput(InputEvent ievent)
    {
        // Alt+Enterによるフルスクリーンへの切り替え
        if (ievent.IsActionPressed("toggle_fullscreen"))
        {
            Fullscreen = !Fullscreen;
            ChangeWindowMode();
        }
    }

    /// <summary>
    /// デフォルト音量にリセットする
    /// </summary>
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
    /// ウィンドウ・フルスクリーン表示を切り替える
    /// ウィンドウの位置とサイズは必要に応じて保存または復元される
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

    /// <summary>
    /// ウィンドウ・フルスクリーン表示を切り替える
    /// ウィンドウの位置とサイズの保存と復元は行わない
    /// </summary>
    public void ChangeOnlyWindowMode()
    {
        if (!Fullscreen && WindowGetMode() is WindowMode.Fullscreen)
        {
            WindowSetMode(WindowMode.Windowed);
        }
        else if (Fullscreen && WindowGetMode() is WindowMode.Windowed)
        {
            WindowSetMode(WindowMode.Fullscreen);
        }
    }

    /// <summary>
    /// ウィンドウの位置とサイズを保存する
    /// </summary>
    public void StoreWindowSizeAndPosition()
    {
        Vector2I windowSize = WindowGetSize();
        Vector2I windowPosition = WindowGetPosition();
        _screenOptions.SetValue("ScreenOptions", "window_width_override", windowSize.X);
        _screenOptions.SetValue("ScreenOptions", "window_height_override", windowSize.Y);
        _screenOptions.SetValue("ScreenOptions", "initial_position", windowPosition);
    }

    /// <summary>
    /// ウィンドウの位置とサイズを復元する
    /// </summary>
    public void RestoreWindowSizeAndPosition()
    {
        int windowWidth = _screenOptions.GetValue("ScreenOptions", "window_width_override", DefaultSizeX).AsInt32();
        int windowHeight = _screenOptions.GetValue("ScreenOptions", "window_height_override", DefaultSizeY).AsInt32();
        Vector2I windowSize = new(windowWidth, windowHeight);
        Vector2I windowPosition = _screenOptions.GetValue("ScreenOptions", "initial_position", DefaultPosition).AsVector2I();
        WindowSetSize(windowSize);
        WindowSetPosition(windowPosition);
    }

    /// <summary>
    /// 適切と思われるウィンドウの位置とサイズを計算する
    /// 実行するとウィンドウモードに切り替わる
    /// </summary>
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
        int windowSizeX = Mathf.FloorToInt(windowContentOnlyOriginalSize.X * scale);
        int windowSizeY = Mathf.FloorToInt(windowContentOnlyOriginalSize.Y * scale);
        // screenOriginを基準にウィンドウ位置を求める
        int windowPositionX = screenOrigin.X + ((screenSize.X - windowSizeX) / 2);
        int windowPositionY = screenOrigin.Y + ((screenSize.Y - windowSizeY) / 2);
        Vector2I windowPosition = new(windowPositionX, windowPositionY);
        _screenOptions.Clear();
        _screenOptions.SetValue("ScreenOptions", "mode", Fullscreen);
        _screenOptions.SetValue("ScreenOptions", "window_width_override", windowSizeX);
        _screenOptions.SetValue("ScreenOptions", "window_height_override", windowSizeY);
        _screenOptions.SetValue("ScreenOptions", "initial_position", windowPosition);
    }

    /// <summary>
    /// ウィンドウの位置とサイズを実際に変更する
    /// </summary>
    public void ApplyScreenOptions()
    {
        Fullscreen = _screenOptions.GetValue("ScreenOptions", "mode", false).AsBool();
        ChangeOnlyWindowMode();

        if (!Fullscreen)
        {
            int windowWidth = _screenOptions.GetValue("ScreenOptions", "window_width_override", DefaultSizeX).AsInt32();
            int windowHeight = _screenOptions.GetValue("ScreenOptions", "window_height_override", DefaultSizeY).AsInt32();
            Vector2I windowSize = new(windowWidth, windowHeight);
            Vector2I windowPosition = _screenOptions.GetValue("ScreenOptions", "initial_position", DefaultPosition).AsVector2I();
            WindowSetSize(windowSize);
            WindowSetPosition(windowPosition);
        }
    }

    /// <summary>
    /// ウィンドウの位置とサイズをConfigFileに保存する
    /// </summary>
    public void SaveScreenOptions()
    {
        _screenOptions.SetValue("ScreenOptions", "mode", Fullscreen);

        if (!Fullscreen)
        {
            Vector2I windowSize = WindowGetSize();
            Vector2I windowPosition = WindowGetPosition();
            _screenOptions.SetValue("ScreenOptions", "window_width_override", windowSize.X);
            _screenOptions.SetValue("ScreenOptions", "window_height_override", windowSize.Y);
            _screenOptions.SetValue("ScreenOptions", "initial_position", windowPosition);
        }

        Error e = _screenOptions.Save(ScreenOptionsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{ScreenOptionsFilePath}を保存できませんでした。エラーコードは{e}です。");
        }
    }

    /// <summary>
    /// ウィンドウの位置とサイズをConfigFileから読み込む
    /// 読み込みに成功した場合はtrue、失敗した場合はfalseを返す
    /// </summary>
    /// <returns>bool</returns>
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
        int windowWidth = _screenOptions.GetValue("ScreenOptions", "window_width_override", DefaultSizeX).AsInt32();
        int windowHeight = _screenOptions.GetValue("ScreenOptions", "window_height_override", DefaultSizeY).AsInt32();
        Vector2I windowPosition = _screenOptions.GetValue("ScreenOptions", "initial_position", DefaultPosition).AsVector2I();
        _screenOptions.SetValue("ScreenOptions", "mode", Fullscreen);
        _screenOptions.SetValue("ScreenOptions", "window_width_override", windowWidth);
        _screenOptions.SetValue("ScreenOptions", "window_height_override", windowHeight);
        _screenOptions.SetValue("ScreenOptions", "initial_position", windowPosition);
        return true;
    }

    /// <summary>
    /// ウィンドウのタイトルバーの上端2点のどちらかが画面内に位置しているか確認する
    /// ウィンドウ一の整理中など一時的にウィンドウを画面外にはみ出して配置することもあるので確認する点は上端2点のみ
    /// 上端2点のどちらかが画面内にある場合true、ない場合falseを返す
    /// ただし、フルスクリーン時には常にtrueを返す
    /// </summary>
    /// <returns>bool</returns>
    private bool IsOnScreen()
    {
        if (Fullscreen)
        {
            return true;
        }

        Vector2I windowPosition = WindowGetPositionWithDecorations();
        Vector2I windowSize = WindowGetSizeWithDecorations();
        int screenCount = GetScreenCount();

        for (int i = 0; i < screenCount; i++)
        {
            Rect2I screenRect = ScreenGetUsableRect(i);

            if (screenRect.HasPoint(windowPosition) || screenRect.HasPoint(windowPosition + new Vector2I(windowSize.X, 0)))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ウィンドウが画面外に飛び出した場合、ウィンドウ位置を再計算して適用する
    /// </summary>
    public void CorrectOnScreen()
    {
        if (!IsOnScreen())
        {
            CalcScreenOptions();
            ApplyScreenOptions();
        }
    }
}
