using Godot;
using Godot.Collections;
using maid_by_shiraishi.screen;
using maid_by_shiraishi.stage;

namespace maid_by_shiraishi.system;

/// <summary>
/// 画面遷移・ダイアログ制御
/// ゲーム全体に影響する最も重要な処理の一つ
/// 
/// スクリーンを開く場合、スタック上のダイアログをすべて閉じ、指定されたスクリーンをメインシーンとして読み込む。
/// ダイアログを開く場合、スタックに新たにダイアログを積む。
/// 
/// 新たにダイアログやスクリーンを開くと、一番前面のダイアログやスクリーンのControlノード内のボタンなどのGUIが無効になり、プロセスが休止される。
/// ダイアログを閉じると、閉じたダイアログのスタックに積まれた次のダイアログのControlノード内あるいはダイアログが開いていない場合はスクリーンのControlノード内のボタンなどのGUIが有効になり、プロセスが再開される。
///
/// ゲームの終了処理も含む。
/// </summary>
public partial class DialogLayer : CanvasLayer
{
    private readonly Array<DialogRoot> _history = [];

    /// <summary>
    /// 現在の画面を返す
    /// </summary>
    /// <returns>ScreenRoot</returns>
    public DialogRoot GetCurrentScreen() => GetTree().CurrentScene as DialogRoot;

    /// <summary>
    /// 現在のダイアログを返す
    /// </summary>
    /// <returns>DialogRoot</returns>
    public DialogRoot GetCurrentDialog() => Pop(false);

    /// <summary>
    /// ダイアログを開く
    /// </summary>
    /// <param name="path">ダイアログパス</param>
    /// <param name="key">引数キー</param>
    /// <param name="argument">引数配列</param>
    public void OpenDialog(string path, string key = null, Variant[] argument = null)
    {
        GetNode<SePlayer>("/root/SePlayer").ClearAllAudioStreamPlayer();

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("ダイアログのパスがnullまたはホワイトスペースです。");
            return;
        }

        if (!string.IsNullOrWhiteSpace(key))
        {
            GetNode<DialogArgument>("/root/DialogArgument").SetArgument(key, argument);
        }

        GetTree().Paused = true;
        _ = CallDeferred(MethodName.DeferredOpenDialog, [path]);
    }

    private void DeferredOpenDialog(string path)
    {
        if (Lib.GetPackedScene(path) is not PackedScene pack || pack.Instantiate() is not DialogRoot dnode)
        {
            GD.PrintErr($"{path}はダイアログではありません。");
            return;
        }

        // ダイアログが開いていないなら画面を停止させる
        if (IsEmpty())
        {
            if (GetCurrentScreen() is DialogRoot screen)
            {
                screen.Inactive();
            }
            else
            {
                GD.PrintErr($"スクリーンが開いていない状態でダイアログを開けません。");
                return;
            }
        }
        // ダイアログが開いているなら停止させる
        else
        {
            DialogRoot dialog = GetCurrentDialog();
            dialog.Inactive();
            dialog.ProcessMode = ProcessModeEnum.Pausable;
        }

        Push(dnode);
        AddChild(dnode);
        dnode.Active();
    }

    /// <summary>
    /// 開いているダイアログをすべて閉じる
    /// </summary>
    /// <param name="undo">アンドゥを実施するか</param>
    public void CloseAllDialog(bool undo = false)
    {
        while (!IsEmpty())
        {
            CloseDialog(undo, true);
        }
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    /// <param name="undo">アンドゥを実施するか</param>
    /// <param name="skipActive">Controlをアクティブに戻さない</param>
    public void CloseDialog(bool undo = false, bool skipActive = false)
    {
        if (Pop(true) is not DialogRoot dnode)
        {
            return;
        }

        if (undo)
        {
            dnode.Undo();
        }

        dnode.Inactive();
        dnode.Close();

        if (IsEmpty())
        {
            if (!skipActive && GetCurrentScreen() is DialogRoot current)
            {
                current.Active();
            }

            GetTree().Paused = false;
        }
        else if (!skipActive && GetCurrentDialog() is DialogRoot dialogRoot)
        {
            dialogRoot.Active();
            dialogRoot.ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    public void UpdateDialogScreen() => GetCurrentDialog()?.UpdateDialogScreen();

    /// <summary>
    /// 画面を開く
    /// </summary>
    /// <param name="path">パス</param>
    /// <param name="fadein">画面のフェードインエフェクト名</param>
    /// <param name="fadeout">画面のフェードアウトエフェクト名</param>
    public void OpenScreen(string path, string fadeout, string fadein, string key = null, Variant[] argument = null)
    {
        GetNode<SePlayer>("/root/SePlayer").ClearAllAudioStreamPlayer();

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("スクリーンのパスがnullまたはホワイトスペースです。");
            return;
        }

        if (!string.IsNullOrWhiteSpace(key))
        {
            GetNode<DialogArgument>("/root/DialogArgument").SetArgument(key, argument);
        }

        GetTree().Paused = true;
        _ = CallDeferred(MethodName.DeferredOpenScreen, [path, fadeout, fadein]);
    }

    protected async void DeferredOpenScreen(string path, string fadeout, string fadein)
    {
        ScreenFader fader = GetNode<ScreenFader>("/root/ScreenFader");
        fader.ScreenFade(fadeout);
        _ = await ToSignal(fader, ScreenFader.SignalName.ScreenFadeFinished);
        CloseAllDialog();
        GetCurrentScreen()?.Inactive();

        if (Lib.GetPackedScene(path) is PackedScene pack)
        {
            _ = GetTree().ChangeSceneToPacked(pack);
        }

        fader.ScreenFade(fadein);
        _ = await ToSignal(fader, ScreenFader.SignalName.ScreenFadeFinished);
        DialogRoot current = GetCurrentScreen();
        current.Active();
        GetTree().Paused = false;
    }

    private bool IsEmpty() => _history.Count == 0;

    private void Push(DialogRoot droot) => _history.Insert(0, droot);

    private DialogRoot Pop(bool delete)
    {
        if (IsEmpty())
        {
            return null;
        }

        DialogRoot droot = _history[0];

        if (delete)
        {
            _history.RemoveAt(0);
        }

        return droot;
    }

    /// <summary>
    /// ゲーム画面を開く
    /// </summary>
    /// <param name="startStageType">ゲーム開始種別</param>
    /// <param name="slotNo">データ番号</param>
    public void OpenGame(StartGameType startStageType, int slotNo, string fadeout, string fadein)
    {
        GetNode<SePlayer>("/root/SePlayer").ClearAllAudioStreamPlayer();
        GetTree().Paused = true;
        _ = CallDeferred(MethodName.DeferredOpenGame, (int)startStageType, slotNo, fadeout, fadein);
    }

    private void DeferredOpenGame(StartGameType startStageType, int slotNo, string fadeout, string fadein)
    {
        GameDataManager gdata = GetNode<GameDataManager>("/root/GameDataManager");
        Error e = Error.Ok;

        // TakeOverStageとRestartは何もしない。
        switch (startStageType)
        {
            case StartGameType.NewStage:

                e = gdata.LoadInitialStartData(GameStageRoot.StageScenarioNo);
                break;

            case StartGameType.NewTutorial:

                e = gdata.LoadInitialStartData(GameStageRoot.TutorialScenarioNo);
                break;

            case StartGameType.Load:

                e = gdata.Load(slotNo);
                break;

            case StartGameType.LoadLastSLot:

                e = gdata.Load();
                break;
        }

        if (e is not Error.Ok)
        {
            string msg = $"ゲームを開始できません。エラーの値は{e}です。";
            GD.PrintErr(msg);
            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/error_dialog.tscn", "ErrorDialog", [msg]);
            return;
        }

        string path = GameStageRoot.GetResourcePath(gdata.GetStageData());

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがNullOrWhiteSpaceです。ChangeSceneToFile()できません。");
            return;
        }

        DeferredOpenScreen(path, fadeout, fadein);
    }

    public void QuitGame() => GetTree().Quit();

    /// <summary>
    /// 現在のゲーム画面を返す
    /// </summary>
    /// <returns>ScreenRoot</returns>
    public GameStageRoot GetCurrentGameStageRoot() => GetTree().CurrentScene as GameStageRoot;
}
