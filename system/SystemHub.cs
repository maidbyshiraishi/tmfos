using Godot;

namespace maid_by_shiraishi.system;

public partial class SystemHub : Node
{
    private static ScreenOption _screenOption;
    private static DialogLayer _dialogLayer;
    private static DialogArgument _dialogArgument;
    private static ScreenFader _screenFader;
    private static GameOption _gameOption;
    private static GameKeyOption _gameKeyOption;
    private static GameDataManager _gameDataManager;
    private static MusicPlayer _musicPlayer;
    private static SePlayer _sePlayer;

    public override void _Ready()
    {
        _screenOption = GetNode<ScreenOption>("ScreenOption");
        _dialogLayer = GetNode<DialogLayer>("DialogLayer");
        _dialogArgument = GetNode<DialogArgument>("DialogArgument");
        _screenFader = GetNode<ScreenFader>("ScreenFader");
        _gameOption = GetNode<GameOption>("GameOption");
        _gameKeyOption = GetNode<GameKeyOption>("GameKeyOption");
        _gameDataManager = GetNode<GameDataManager>("GameDataManager");
        _musicPlayer = GetNode<MusicPlayer>("MusicPlayer");
        _sePlayer = GetNode<SePlayer>("SePlayer");
    }

    public static ScreenOption GetScreenOption() => _screenOption;
    public static DialogLayer GetDialogLayer() => _dialogLayer;
    public static DialogArgument GetDialogArgument() => _dialogArgument;
    public static ScreenFader GetScreenFader() => _screenFader;
    public static GameOption GetGameOption() => _gameOption;
    public static GameKeyOption GetGameKeyOption() => _gameKeyOption;
    public static GameDataManager GetGameDataManager() => _gameDataManager;
    public static MusicPlayer GetMusicPlayer() => _musicPlayer;
    public static SePlayer GetSePlayer() => _sePlayer;
}
