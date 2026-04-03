using Godot;

namespace maid_by_shiraishi.command;

/// <summary>
/// コマンドノードインタフェース
/// </summary>
public interface ICommand
{
    /// <summary>
    /// コマンドを実行する
    /// </summary>
    /// <param name="node">Node</param>
    /// <param name="flag">裏表フラグ</param>
    void ExecCommand(Node node, bool flag);
}
