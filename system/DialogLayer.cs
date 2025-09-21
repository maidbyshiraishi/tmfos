using Godot;
using Godot.Collections;
using tmfos.screen;
using tmfos.stage;

namespace tmfos.system;

/// <summary>
/// 画面遷移・ダイアログ制御
/// </summary>
public partial class DialogLayer : CanvasLayer
{
    [Export]
    public bool MultipleOpen { get; set; } = false;

    private readonly Array<DialogRoot> _history = [];

    /// <summary>
    /// 現在の画面を返す
    /// </summary>
    /// <returns>ScreenRoot</returns>
    public DialogRoot GetCurrentScreen()
    {
        return GetTree().CurrentScene as DialogRoot;
    }

    /// <summary>
    /// 現在のゲーム画面を返す
    /// </summary>
    /// <returns>ScreenRoot</returns>
    public StageRoot GetCurrentStageRoot()
    {
        return GetTree().CurrentScene as StageRoot;
    }

    /// <summary>
    /// 現在のダイアログを返す
    /// </summary>
    /// <returns>DialogRoot</returns>
    public DialogRoot GetCurrentDialog()
    {
        return Pop(false);
    }

    /// <summary>
    /// ダイアログを開く
    /// </summary>
    /// <param name="path">ダイアログパス</param>
    /// <param name="key">引数キー</param>
    /// <param name="argument">引数配列</param>
    public void OpenDialog(string path, string key = null, Variant[] argument = null)
    {
        GetNode<SePlayer>("/root/SePlayer").ClearAllAudioStreamPlayer();
        GetTree().Paused = true;

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがNullOrWhiteSpaceです。OpenDialog()を実行できません。");
            return;
        }

        if (!string.IsNullOrWhiteSpace(key))
        {
            GetNode<GameArgument>("/root/GameArgument").SetArgument(key, argument);
        }

        _ = CallDeferred(MethodName.DeferredOpenDialog, path);
    }

    private void DeferredOpenDialog(string path)
    {
        if (Lib.GetPackedScene<PackedScene>(path) is not PackedScene pack || pack.Instantiate() is not Node node)
        {
            return;
        }

        DialogRoot screen = GetCurrentScreen();

        if (screen is null)
        {
            GD.PrintErr($"スクリーンが開いていない状態でOpenDialog()を実行できません。");
            return;
        }

        if (node is not DialogRoot dnode)
        {
            GD.PrintErr($"{path}はダイアログではありません。OpenDialog()を実行できません。");
            return;
        }

        // ダイアログが開いていないなら画面を停止させる
        if (IsEmpty())
        {
            screen.Inactive();
            GetTree().Paused = true;
        }
        // ダイアログが開いているなら停止させる
        else
        {
            GetCurrentDialog().Inactive();
            GetCurrentDialog().ProcessMode = ProcessModeEnum.Pausable;
        }

        Push(dnode);
        AddChild(dnode);
        GetCurrentDialog().Active();
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
        DialogRoot dnode = Pop(true);

        if (dnode is null)
        {
            return;
        }

        if (undo)
        {
            dnode.Undo();
        }

        dnode.Close();

        if (IsEmpty())
        {
            if (!skipActive)
            {
                GetCurrentScreen()?.Active();
            }

            GetTree().SetDeferred(SceneTree.PropertyName.Paused, false);
        }
        else if (!skipActive)
        {
            DialogRoot dialogRoot = GetCurrentDialog();

            if (dialogRoot is not null)
            {
                dialogRoot.Active();
                dialogRoot.ProcessMode = ProcessModeEnum.Inherit;
            }
        }
    }

    public void UpdateDialogScreen()
    {
        GetCurrentDialog()?.UpdateDialogScreen();
    }

    /// <summary>
    /// 画面を開く
    /// </summary>
    /// <param name="path">パス</param>
    public void OpenScreen(string path, string fadeout, string fadein)
    {
        GetNode<SePlayer>("/root/SePlayer").ClearAllAudioStreamPlayer();

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがNullOrWhiteSpaceです。ChangeSceneToFile()できません。");
            return;
        }

        GetTree().Paused = true;
        DialogRoot dialog = GetCurrentDialog();

        if (dialog is null)
        {
            DialogRoot screen = GetCurrentScreen();
            screen?.Inactive();
        }
        else
        {
            dialog.Inactive();
        }

        _ = CallDeferred(MethodName.DeferredOpenScreen, path, fadeout, fadein);
    }

    private async void DeferredOpenScreen(string path, string fadeout, string fadein)
    {
        ScreenFader fader = GetNode<ScreenFader>("/root/ScreenFader");
        fader.ScreenFade(fadeout);
        _ = await ToSignal(fader, ScreenFader.SignalName.ScreenFadeFinished);
        CloseAllDialog();

        if (Lib.GetPackedScene<PackedScene>(path) is PackedScene pack)
        {
            _ = GetTree().ChangeSceneToPacked(pack);
        }

        fader.ScreenFade(fadein);
        _ = await ToSignal(fader, ScreenFader.SignalName.ScreenFadeFinished);
        GetCurrentScreen().Active();
        GetTree().Paused = false;
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
        GameData gdata = GetNode<GameData>("/root/GameData");
        Error e = Error.Ok;

        // TakeOverStageとRestartは何もしない。
        switch (startStageType)
        {
            case StartGameType.NewStage:

                e = gdata.LoadInitialStartData(StageRoot.StageScenarioNo);
                break;

            case StartGameType.NewTutorial:

                e = gdata.LoadInitialStartData(StageRoot.TutorialScenarioNo);
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

        string path = StageRoot.GetResourcePath(gdata.GetStageData());

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがNullOrWhiteSpaceです。ChangeSceneToFile()できません。");
            return;
        }

        DeferredOpenScreen(path, fadeout, fadein);
    }

    private bool IsEmpty()
    {
        return _history.Count == 0;
    }

    private void Push(DialogRoot droot)
    {
        _history.Insert(0, droot);
    }

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

    public void QuitGame()
    {
        GetTree().Quit();
    }
}
