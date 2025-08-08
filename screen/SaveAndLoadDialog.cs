using Godot;
using tmfos.command;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// セーブとロードダイアログ
/// </summary>
public partial class SaveAndLoadDialog : DialogRoot
{
    public override void Active()
    {
        base.Active();
        UpdateTimeStamp();
    }

    private void UpdateTimeStamp()
    {
        GameData gdata = GetNode<GameData>("/root/GameData");
        string[] date = gdata.GetFileDates();

        for (int i = 1; i <= GameData.NumOfSaveFiles; i++)
        {
            if (date[i - 1] is not null)
            {
                GetNode<Control>("Control/Load").Show();
                break;
            }
        }

        if (gdata.HasLastSlotNo())
        {
            int lastSlotNo = gdata.LastSlotNo;
            GetNode<Control>("Control/LastLoad").Show();
            OpenLoadConfirmDialogCommand load = GetNode<OpenLoadConfirmDialogCommand>("Control/LastLoad/Exec/OpenLoadConfirmDialogCommand");
            load.SlotNo = lastSlotNo;
            OpenSaveConfirmDialogCommand save = GetNode<OpenSaveConfirmDialogCommand>("Control/LastSave/Exec/OpenSaveConfirmDialogCommand");
            save.SlotNo = lastSlotNo;
            GetNode<Control>("Control/LastSave").Show();
        }
        else
        {
            GetNode<Control>("Control/LastLoad").Hide();
            GetNode<Control>("Control/LastSave").Hide();
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
