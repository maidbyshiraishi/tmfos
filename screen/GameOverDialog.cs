using Godot;
using maid_by_shiraishi.command.dialog;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.screen;

/// <summary>
/// ゲームオーバーダイアログ
/// </summary>
public partial class GameOverDialog : DialogRoot
{
    protected override void InitializeNode()
    {
        GameDataManager gdata = GetNode<GameDataManager>("/root/GameDataManager");
        string[] date = gdata.GetFileDates();

        for (int i = 1; i <= GameDataManager.NumOfSaveFiles; i++)
        {
            if (date[i - 1] is not null)
            {
                GetNode<Control>("Control/Load").Show();
                break;
            }
        }

        if (gdata.HasLastSlotNo())
        {
            OpenLoadConfirmDialogCommand command = GetNode<OpenLoadConfirmDialogCommand>("Control/LastLoad/Exec/OpenLoadConfirmDialogCommand");
            command.SlotNo = gdata.LastSlotNo;
            GetNode<Control>("Control/LastLoad").Show();
        }
        else
        {
            GetNode<Control>("Control/LastLoad").Hide();
        }
    }
}
