using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command;

/// <summary>
/// コマンド延滞実行コンテナ
/// </summary>
public partial class DelayContainer : CommandRoot
{
    /// <summary>
    /// 延滞時間
    /// </summary>
    [Export]
    public double WaitTime { get; set; } = 0f;

    private bool _start = false;
    private double _waitTime = 0f;
    private double _count = 0f;
    private Node _node;
    private bool _flag = false;

    public override void DoCommand(Node node, bool flag)
    {
        _waitTime = WaitTime;
        _count = 0f;
        _node = node;
        _flag = flag;
        _start = true;
    }

    public override void _Process(double delta)
    {
        if (_start)
        {
            _count += delta;

            if (_waitTime <= _count)
            {
                _start = false;
                Lib.ExecCommands(this, _node, _flag);
            }
        }
    }
}
