using Godot;

namespace tmfos.command;

/// <summary>
/// システムのファイルマネージャで指定フォルダを開くコマンド
/// </summary>
public partial class OpenFileManagerCommand : CommandNode
{
    /// <summary>
    /// 開くフォルダ
    /// </summary>
    [Export]
    public string Path { get; set; } = "user://";

    public override void DoCommand(Node node, bool flag)
    {
        string path = ProjectSettings.GlobalizePath(Path);

        if (DirAccess.DirExistsAbsolute(path))
        {
            _ = OS.ShellShowInFileManager(path);
        }
    }
}
