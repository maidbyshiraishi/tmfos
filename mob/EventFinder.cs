using Godot;
using Godot.Collections;
using tmfos.door;
using tmfos.stage;
using tmfos.trigger;

namespace tmfos.mob;

/// <summary>
/// イベント処理との接触判定を行う。
/// </summary>
public partial class EventFinder : Area2D
{
    public Node2D EventNode2D;
    private readonly Array<ulong> _target = [];

    public override void _Ready()
    {
        _ = Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.Area2DEntered));
        _ = Connect(Area2D.SignalName.AreaExited, new(this, MethodName.Area2DExited));
        AddToGroup(StageRoot.PhysicsProcessGroup);

        if (GetParent() is ActionMob amob)
        {
            EventNode2D = amob;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (EventNode2D is null)
        {
            return;
        }

        Array<ulong> remove = [];

        foreach (ulong id in _target)
        {
            if (!IsInstanceIdValid(id))
            {
                remove.Add(id);
                continue;
            }

            GodotObject gobj = InstanceFromId(id);

            if (!IsInstanceValid(gobj))
            {
                remove.Add(id);
                continue;
            }

            if (gobj is TriggerArea2D node && OverlapsArea(node))
            {
                // 対象ノードが調査で反応する場合
                if (node.Search)
                {
                    // EventNode2Dが調査を行っている場合のみ
                    if (EventNode2D is ActionMob amob && amob.Search)
                    {
                        // EventNode2Dが死亡している場合
                        if (EventNode2D is DurableMob dmob && dmob.MobState == MobStateType.Dead)
                        {
                            // Warpはイベントが発生しないが、Gatewayなら発生する
                            if (node is not Warp and Gateway)
                            {
                                node.Exec(EventNode2D);
                            }
                        }
                        // EventNode2Dが死亡していない場合、イベントが発生する
                        else
                        {
                            node.Exec(EventNode2D);
                        }
                    }
                }
                // 対象ノードが調査しなくても反応する場合、イベントが発生
                else
                {
                    node.Exec(EventNode2D);
                }
            }
        }

        foreach (ulong id in remove)
        {
            _ = _target.Remove(id);
        }
    }

    public void Area2DEntered(Area2D area)
    {
        _ = CallDeferred(MethodName.DeferredNodeEntered, area);
    }

    public void Area2DExited(Area2D area)
    {
        _ = CallDeferred(MethodName.DeferredNodeExited, area);
    }

    public void DeferredNodeEntered(Area2D node)
    {
        ulong id = node.GetInstanceId();

        if (!_target.Contains(id))
        {
            _target.Add(id);
        }
    }

    public void DeferredNodeExited(Area2D node)
    {
        ulong id = node.GetInstanceId();

        if (_target.Contains(id))
        {
            _ = _target.Remove(id);
        }
    }

    public void Exec()
    {
        //Lib.ExecCommands(node, EventNode2D, true);
    }
}
