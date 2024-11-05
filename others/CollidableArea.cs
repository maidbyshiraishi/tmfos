using Godot;
using Godot.Collections;
using tmfos.mob;
using tmfos.stage;

namespace tmfos.others;

/// <summary>
/// 接触エリア
/// オブジェクトが削除される場合は使用できない
/// </summary>
public partial class CollidableArea : Area2D
{
    [Signal]
    public delegate void CollidedEventHandler(Node2D node, Area2D area);

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
    /// 調査で発動
    /// </summary>
    [Export]
    public bool Search { get; set; } = false;

    private readonly Array<ulong> _target = [];
    private bool _wait = false;
    private readonly bool[] _mask = new bool[32];

    public override void _Ready()
    {
        _ = Connect(Area2D.SignalName.BodyEntered, new Callable(this, MethodName.NodeEntered));
        _ = Connect(Area2D.SignalName.BodyExited, new Callable(this, MethodName.NodeExited));
        _ = Connect(Area2D.SignalName.AreaEntered, new Callable(this, MethodName.Area2DEntered));
        _ = Connect(Area2D.SignalName.AreaExited, new Callable(this, MethodName.Area2DExited));
        AddToGroup(StageRoot.PhysicsProcessGroup);
        BackupCollisionMaskValue();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_wait)
        {
            return;
        }

        Array<ulong> remove = [];

        // todo: キャラクターが同時にOutOfBorderとFireに接触した場合、InvalidCastExceptionが発生する。OutOfBorderの仕様変更で対応し、根本解決していない。
        // System.InvalidCastException: Unable to cast object of type 'Godot.GodotObject' to type 'Godot.Node2D'.

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

            if (gobj is Node2D node && (OverlapsArea(node) || OverlapsBody(node)) && (!Search || (Search && node is ActionMob amob && amob.Search) || node.Name == "ItemSearch"))
            {
                _ = EmitSignal(SignalName.Collided, node, this);
            }
        }

        foreach (ulong id in remove)
        {
            _ = _target.Remove(id);
        }

        if (!Continuous)
        {
            _target.Clear();
        }

        Wait();
    }

    private void BackupCollisionMaskValue()
    {
        for (int i = 0; i < 32; i++)
        {
            _mask[i] = GetCollisionMaskValue(i + 1);
        }
    }

    private void DisableCollisionMask()
    {
        BackupCollisionMaskValue();

        for (int i = 0; i < 32; i++)
        {
            SetCollisionMaskValue(i + 1, false);
        }

        SetPhysicsProcess(false);
    }

    private void EnableCollisionMask()
    {
        for (int i = 0; i < 32; i++)
        {
            SetCollisionMaskValue(i + 1, _mask[i]);
        }

        SetPhysicsProcess(true);
    }

    public void NodeEntered(Node2D node)
    {
        _ = CallDeferred(MethodName.DeferredNodeEntered, node);
    }

    public void DeferredNodeEntered(Node2D node)
    {
        ulong id = node.GetInstanceId();

        if (!_target.Contains(id))
        {
            _target.Add(id);
        }
    }

    public void NodeExited(Node2D node)
    {
        _ = CallDeferred(MethodName.DeferredNodeExited, node);
    }

    public void DeferredNodeExited(Node2D node)
    {
        ulong id = node.GetInstanceId();

        if (_target.Contains(id))
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

    protected async void Wait()
    {
        if (WaitTime >= 0.05f)
        {
            _wait = true;
            _ = await ToSignal(GetTree().CreateTimer(WaitTime), Timer.SignalName.Timeout);
        }

        _wait = false;
    }
}
