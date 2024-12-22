using Godot;
using Godot.Collections;
using tmfos.stage;

namespace tmfos.others;

/// <summary>
/// パスに沿って動くもの
/// </summary>
public partial class PathFollow : PathFollow2D
{
    [Export]
    public EdgeHandlingType EdgeHandling { get; set; } = EdgeHandlingType.Shuttle;

    [Export]
    public bool Reverse { get; set; } = false;

    [Export]
    public float Speed { get; set; } = 100f;

    [Export]
    public bool ParentPathLooped { get; set; }

    public override void _Ready()
    {
        AddToGroup(StageRoot.PhysicsProcessGroup);
    }

    public override void _PhysicsProcess(double delta)
    {
        Progress += (Reverse ? -1f : 1f) * Speed * (float)delta;

        switch (EdgeHandling)
        {
            case EdgeHandlingType.Oneway:

                ExecOneway();
                break;

            case EdgeHandlingType.Shuttle:

                ExecShuttle();
                break;

            case EdgeHandlingType.Loop:

                ExecLoop();
                break;
        }
    }

    protected virtual void ExecOneway()
    {
        if (ProgressRatio is 1.0f or 0.0f)
        {
            ReachedToEdge();
        }
    }

    protected virtual void ExecShuttle()
    {
        if (ProgressRatio is 1.0f or 0.0f)
        {
            Reverse = !Reverse;
            ReachedToEdge();
        }
    }

    protected virtual void ExecLoop()
    {
        if (ProgressRatio == 0.0f)
        {
            ProgressRatio = 1.0f;
            ReachedToEdge();
        }
        else if (ProgressRatio == 1.0f)
        {
            ProgressRatio = 0.0f;
            ReachedToEdge();
        }
    }

    protected virtual void ReachedToEdge()
    {
        Array<Node> nodes = GetChildren();

        foreach (Node n in nodes)
        {
            if (n is IPathFollower pfollower)
            {
                pfollower.ReachedToEdge(ProgressRatio, Reverse, EdgeHandling);
            }
        }
    }
}
