using Godot;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// ステージ情報を保存するコマンド
/// </summary>
public partial class StateBackupCommand : CommandNode
{
    public override void DoCommand(Node node, bool flag)
    {
        GetNode<GameData>("/root/GameData").Backup();
    }
}
