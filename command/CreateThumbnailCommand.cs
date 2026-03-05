using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// 画面サムネイル生成コマンド
/// </summary>
public partial class CreateThumbnailCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        Image image = GetViewport().GetTexture().GetImage();
        image.Resize(128, 96);
        GetNode<GameData>("/root/GameData").SetThumbnail(image);
    }
}
