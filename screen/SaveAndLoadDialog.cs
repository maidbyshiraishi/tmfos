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

        if (gdata.HasLastSlotNo())
        {
            int lastSlotNo = gdata.LastSlotNo;
            GetNode<BaseButton>("Control/LastLoad").Disabled = false;
            OpenLoadConfirmDialogCommand load = GetNode<OpenLoadConfirmDialogCommand>("Control/LastLoad/Exec/OpenLoadConfirmDialogCommand");
            load.SlotNo = lastSlotNo;
            OpenSaveConfirmDialogCommand save = GetNode<OpenSaveConfirmDialogCommand>("Control/LastSave/Exec/OpenSaveConfirmDialogCommand");
            save.SlotNo = lastSlotNo;
            GetNode<BaseButton>("Control/LastSave").Disabled = false;
        }
        else
        {
            GetNode<BaseButton>("Control/LastLoad").Disabled = true;
            GetNode<BaseButton>("Control/LastSave").Disabled = true;
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
