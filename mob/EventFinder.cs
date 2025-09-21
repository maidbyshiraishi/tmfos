using Godot;
using Godot.Collections;
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

            if (gobj is TriggerArea2D node && OverlapsArea(node) && (!node.Search || (node.Search && EventNode2D is ActionMob amob && amob.Search)))
            {
                node.Exec(EventNode2D);
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
