using Godot;
using System;

namespace maid_by_shiraishi.command;

/// <summary>
/// スクリーンショット保存コマンド
/// </summary>
public partial class SaveScreenshotCommand : CommandRoot
{
    [Export]
    public string ScreenshotPath { get; set; } = "user://{0}_{1}.png";

    public override void DoCommand(Node node, bool flag)
    {
        Image image = GetViewport().GetTexture().GetImage();
        DateTime datetime = DateTime.Now;
        string datetimeString = datetime.ToString("yyyyMMddHHmmss");
        string projectName = ProjectSettings.GetSetting("application/config/name", "screenshot").AsString();
        string file = string.Format(ScreenshotPath, [projectName, datetimeString]);
        Error e = image.SavePng(file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"スクリーンショットの保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
        }
    }
}
