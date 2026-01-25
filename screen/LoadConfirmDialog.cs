using Godot;
using tmfos.command.dialog;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// ロード確認ダイアログ
/// </summary>
public partial class LoadConfirmDialog : DialogRoot
{
    public override void GetArgument()
    {
        GetGameArgument("LoadConfirmDialog");

        if (m_argument is null || m_argument.Length != 1 || m_argument[0].VariantType is not Variant.Type.Int)
        {
            return;
        }

        int slotNo = (int)m_argument[0];
        GetNode<Label>("Data").Text = $"データ{slotNo}";
        string date = GetNode<GameData>("/root/GameData").GetFileDate(slotNo);
        Button yesButton = GetNode<Button>("Control/Yes");

        if (date is null)
        {
            yesButton.Disabled = true;
            GetNode<Label>("Date").Text = "ロードできません。";
        }
        else
        {
            yesButton.Disabled = false;
            GetNode<Label>("Date").Text = date;
            OpenGameCommand command = GetNode<OpenGameCommand>("Control/Yes/Exec/OpenGameCommand");
            command.SlotNo = slotNo;
            string fileThumbnail = string.Format(GameData.DataThumbnailPath, slotNo);

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
}
