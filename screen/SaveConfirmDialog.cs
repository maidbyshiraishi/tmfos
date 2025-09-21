using Godot;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// セーブ確認ダイアログ
/// </summary>
public partial class SaveConfirmDialog : DialogRoot
{
    private int _slotNo;

    public override void GetArgument()
    {
        GetGameArgument("SaveConfirmDialog");

        if (m_argument is null || m_argument.Length != 1 || m_argument[0].VariantType is not Variant.Type.Int)
        {
            return;
        }

        _slotNo = (int)m_argument[0];
        GetNode<Label>("Data").Text = $"データ{_slotNo}";
        string date = GetNode<GameData>("/root/GameData").GetFileDate(_slotNo);

        if (date is null)
        {
            GetNode<Label>("Date").Text = "新規";
        }
        else
        {
            GetNode<Label>("Date").Text = date;
            string fileThumbnail = string.Format(GameData.DataThumbnailPath, _slotNo);

            if (FileAccess.FileExists(fileThumbnail))
            {
                Sprite2D sprite = GetNode<Sprite2D>($"Sprite2D");
                Image image = Image.LoadFromFile(fileThumbnail);
                ImageTexture texture = ImageTexture.CreateFromImage(image);
                sprite.Texture = texture;
            }
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "No";
    }

    /// <summary>
    /// はい
    /// </summary>
    public void Yes()
    {
        if (_slotNo < 0 || GameData.NumOfSaveFiles < _slotNo)
        {
            string msg = $"ゲームデータのセーブ中にエラーが発生しました。エラーの発生したデータはゲームデータ{_slotNo}です。";
            GD.PrintErr(msg);
            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/error_dialog.tscn", "ErrorDialog", [msg]);
            return;
        }

        Error e = GetNode<GameData>("/root/GameData").Save(_slotNo);

        if (e is not Error.Ok)
        {
            string msg = $"ゲームデータのセーブ中にエラーが発生しました。エラーの発生したデータはゲームデータ{_slotNo}です。エラーの値は{e}です。";
            GD.PrintErr(msg);
            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/error_dialog.tscn", "ErrorDialog", [msg]);
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").CloseDialog();
    }
}
