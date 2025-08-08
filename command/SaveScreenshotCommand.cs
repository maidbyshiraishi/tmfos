using Godot;
using System;

namespace tmfos.command;

/// <summary>
/// スクリーンショット保存コマンド
/// </summary>
public partial class SaveScreenshotCommand : CommandNode
{
    private static readonly string ScreenshotPath = "user://tmfos_{0}.png";

    public override void DoCommand(Node node, bool flag)
    {
        Image image = GetViewport().GetTexture().GetImage();
        DateTime datetime = DateTime.Now;
        string datetimeString = datetime.ToString("yyyyMMddHHmmss");
        string file = string.Format(ScreenshotPath, datetimeString);
        Error e = image.SavePng(file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"スクリーンショットの保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
        }
    }
}
