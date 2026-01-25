using Godot;
using System.Collections.Generic;
using tmfos.command.stage;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// 探索トリガー
/// </summary>
public partial class SearchTrigger : TriggerArea2D, IStateful
{
    /// <summary>
    /// 状態
    /// </summary>
    [Export]
    public bool Switch { get; set; } = false;

    private bool _disable = false;
    private readonly Dictionary<Node2D, VisibleOnScreenEnabler2D> _enablerList = [];

    public override void _Ready()
    {
        MakeEnabler();
        base._Ready();
    }

    public override void Exec(Node2D node)
    {
        if (_disable)
        {
            return;
        }

        _disable = true;
        Switch = !Switch;
        StateSave();
        SwitchNode();
        Lib.ExecCommands(this, null, Switch);
        ResetDisable();
    }

    private async void ResetDisable()
    {
        _ = await ToSignal(GetTree().CreateTimer(0.5f), Timer.SignalName.Timeout);
        _disable = false;
    }

    private void MakeEnabler()
    {
        CanvasLayer clayer = GetNode<CanvasLayer>("../SearchList");

        foreach (Node child in GetChildren())
        {
            if (child is not SearchContainer container)
            {
                continue;
            }

            foreach (Node cnode in container.GetChildren())
            {
                if (cnode is not Node2D node2d)
                {
                    continue;
                }

                VisibleOnScreenEnabler2D enabler = new()
                {
                    EnableNodePath = node2d.GetPath(),
                    Position = new(320f, 240f)
                };

                _enablerList.Add(node2d, enabler);
                clayer.AddChild(enabler);
            }
        }
    }

    protected void SwitchNode()
    {
        foreach (Node2D child in _enablerList.Keys)
        {
            if (Switch)
            {
                _enablerList[child].Show();
                child.Show();
            }
            else
            {
                _enablerList[child].Hide();
                child.Hide();
            }
        }
    }

    public override void StateSave()
    {
        if (!Stateful)
        {
            return;
        }

        GameData gdata = GetNode<GameData>("/root/GameData");
        string name = Lib.GenerateName(this);
        int stageNo = gdata.GetStageData().StageNo;
        gdata.SetStageData(stageNo, $"{name}_Switch", Switch ? 1 : 0);
    }

    public override void StateLoad()
    {
        if (!Stateful)
        {
            return;
        }

        GameData gdata = GetNode<GameData>("/root/GameData");
        string name = Lib.GenerateName(this);
        int stageNo = gdata.GetStageData().StageNo;

        if (gdata.HasStageData(stageNo, $"{name}_Switch"))
        {
            Switch = gdata.GetStageData(stageNo, $"{name}_Switch") == 1;
        }

        SwitchNode();
    }
}
