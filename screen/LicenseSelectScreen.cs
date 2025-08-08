using Godot;
using Godot.Collections;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// ライセンス選択画面
/// </summary>
public partial class LicenseSelectScreen : DialogRoot
{
    private ItemList _licenseList;

    protected override void InitializeNode()
    {
        _licenseList = GetNode<ItemList>("Control/LicenseList");
        AddItemLicenseList();
    }

    public void SelectLicense(int index)
    {
        GetNode<SePlayer>("/root/SePlayer").Play("menu_select");
        string key = _licenseList.GetItemText(index);
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/license_dialog.tscn", "LicenseDialog", [key]);
    }

    private void AddItemLicenseList()
    {
        Array<Dictionary> components = Engine.GetCopyrightInfo();

        foreach (Dictionary component in components)
        {
            _ = _licenseList.AddItem(component["name"].AsString());
        }

        _ = _licenseList.AddItem("PixelMplus");
        _ = _licenseList.AddItem("VOICEVOX　四国めたん");
        _ = _licenseList.AddItem("宇宙から来たメイド");
    }
}
