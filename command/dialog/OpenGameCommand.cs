using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.command.dialog;

/// <summary>
/// ゲームを開始するコマンド
/// </summary>
public partial class OpenGameCommand : CommandNode
{
    /// <summary>
    /// フェードアウトエフェクト
    /// </summary>
    [Export]
    public string Fadeout { get; set; } = "fadeout_1";

    /// <summary>
    /// フェードインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = "fadein_1";

    /// <summary>
    /// ゲーム開始種別
    /// </summary>
    [Export]
    public StartGameType StartGame { get; set; } = StartGameType.NewStage;

    /// <summary>
    /// ゲームデータ番号
    /// </summary>
    [Export]
    public int SlotNo { get; set; } = GameData.DefaultSlotNo;

    public override void DoCommand(Node node, bool flag)
    {
        if (StartGame is StartGameType.Load)
        {
            if (SlotNo < 0 || GameData.NumOfSaveFiles < SlotNo)
            {
                string msg = $"ゲームデータのロード中にエラーが発生しました。エラーの発生したデータはゲームデータ{SlotNo}です。";
                GD.PrintErr(msg);
                GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/error_dialog.tscn", "ErrorDialog", [msg]);
                return;
            }
        }
        else if (StartGame is StartGameType.NewStage or StartGameType.NewTutorial or StartGameType.TakeoverStage or StartGameType.Restart or StartGameType.LoadLastSLot)
        {
            SlotNo = GameData.DefaultSlotNo;
        }

        GetNode<DialogLayer>("/root/DialogLayer").OpenGame(StartGame, SlotNo, Fadeout, Fadein);
    }
}
