using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.state;

/// <summary>
/// ステージ情報を保存するコマンド
/// </summary>
public partial class StateBackupCommand : CommandRoot
{
    public override void DoCommand(Node node, bool flag) => GetNode<GameDataManager>("/root/GameDataManager").Backup();
}
