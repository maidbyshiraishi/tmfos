using Godot;
using tmfos.player;
using tmfos.stage;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// トリガーArea2D版
/// 検出される側であり、接触しても、こちら側からは何も行わず、接触側のEventFinderでコマンドの呼び出しなどを行う。
/// </summary>
public partial class TriggerArea2D : Area2D, IStateful
{
    /// <summary>
    /// 接触が継続して行われるか
    /// </summary>
    [Export]
    public bool Continuous { get; set; } = false;

    /// <summary>
    /// 継続接触の間隔
    /// </summary>
    [Export]
    public double WaitTime { get; set; }

    /// <summary>
    /// 有効・無効
    /// </summary>
    [Export]
    public bool Disable { get; set; } = false;

    /// <summary>
    /// 調査で発動
    /// </summary>
    [Export]
    public bool Search { get; set; } = false;

    /// <summary>
    /// ステージをまたいで状態を保存するか
    /// </summary>
    [Export]
    public bool Stateful { get; set; } = true;

    /// <summary>
    /// 一度のみトリガーを実行する
    /// </summary>
    [Export]
    public bool OneTime { get; set; } = true;

    /// <summary>
    /// プレーヤーにのみ反応する
    /// </summary>
    [Export]
    public bool PlayerOnly { get; set; } = true;

    protected bool m_opened = false;

    public override void _Ready()
    {
        if (Stateful)
        {
            AddToGroup(StageRoot.StatefulGroup);
        }
    }

    public virtual void Exec(Node2D node)
    {
        if (Disable || (OneTime && m_opened) || (PlayerOnly && node is not Player))
        {
            return;
        }

        if (OneTime)
        {
            m_opened = true;
        }

        Lib.ExecCommands(this, node, true);
    }

    public virtual void StateLoad()
    {
        GameData gdata = GetNode<GameData>("/root/GameData");
        string name = Lib.GenerateName(this);
        int stageNo = gdata.GetStageData().StageNo;

        if (gdata.HasStageData(stageNo, $"{name}_Opened"))
        {
            SetOpened(gdata.GetStageData(stageNo, $"{name}_Opened") == 1);
        }
    }

    public virtual void StateSave()
    {
        GameData gdata = GetNode<GameData>("/root/GameData");
        string name = Lib.GenerateName(this);
        int stageNo = gdata.GetStageData().StageNo;
        gdata.SetStageData(stageNo, $"{name}_Opened", m_opened ? 1 : 0);
    }

    public virtual void SetOpened(bool opened)
    {
        m_opened = opened;
    }
}
