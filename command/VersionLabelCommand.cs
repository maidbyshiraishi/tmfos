using Godot;

namespace maid_by_shiraishi.command;

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

    public override void _Ready()
    {
        base._Ready();

        if (VersionLabel is null && GetParent() is Label label)
        {
            VersionLabel = label;
            DoCommand(null, true);
        }
    }

    public override void DoCommand(Node node, bool flag)
    {
        if (VersionLabel is not null)
        {
            VersionLabel.Text = ProjectSettings.GetSetting("application/config/version").AsString();
        }
    }
}
