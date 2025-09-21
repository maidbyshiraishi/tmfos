using Godot;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// ロードダイアログ
/// </summary>
public partial class LoadDialog : DialogRoot
{
    public override void Active()
    {
        UpdateTimeStamp();
        base.Active();
    }

    private void UpdateTimeStamp()
    {
        string[] date = GetNode<GameData>("/root/GameData").GetFileDates();

        for (int i = 1; i <= GameData.NumOfSaveFiles; i++)
        {
            Button b = GetNode<Button>($"Control/Data{i}");
            Label l = GetNode<Label>($"Date{i}");

            if (date[i - 1] is null)
            {
                b.Disabled = true;
                l.Text = "新規";
                continue;
            }

            b.Disabled = false;
            l.Text = date[i - 1];

            string fileThumbnail = string.Format(GameData.DataThumbnailPath, i);

            if (!FileAccess.FileExists(fileThumbnail))
            {
                continue;
            }

            Sprite2D sprite = GetNode<Sprite2D>($"Sprite2D{i}");
            Image image = Image.LoadFromFile(fileThumbnail);
            ImageTexture texture = ImageTexture.CreateFromImage(image);
            sprite.Texture = texture;
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
