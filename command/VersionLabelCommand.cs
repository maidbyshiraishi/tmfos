using Godot;

namespace tmfos.command;

/// <summary>
/// ラベルの文字にバージョンを表示するコマンド
/// </summary>
public partial class VersionLabelCommand : CommandRoot
{
    /// <summary>
    /// バージョンを表示するラベル
    /// </summary>
    [Export]
    public Label VersionLabel { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (VersionLabel is not null)
        {
            VersionLabel.Text = ProjectSettings.GetSetting("application/config/version").AsString();
        }
    }
}
