using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command;

/// <summary>
/// 画面サムネイル生成コマンド
/// </summary>
public partial class CreateThumbnailCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag)
    {
        Image image = GetViewport().GetTexture().GetImage();
        image.Resize(128, 96);
        GetNode<GameDataManager>("/root/GameDataManager").SetThumbnail(image);
    }
}
