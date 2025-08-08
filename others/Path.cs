using Godot;
using Godot.Collections;

namespace tmfos.others;

/// <summary>
/// 終端処理を行うパス
/// </summary>
public partial class Path : Path2D
{
    public override void _Ready()
    {
        SetChildrenPathLooped();
    }

    protected void SetChildrenPathLooped()
    {
        bool looped;
        Curve2D curve = Curve;

        if (curve is null)
        {
            looped = true;
        }
        else
        {
            int count = curve.PointCount;
            looped = count == 0 || curve.GetPointPosition(0) == curve.GetPointPosition(count - 1);
        }

        Array<Node> nodes = GetChildren();

        foreach (Node n in nodes)
        {
            if (n is PathFollow pfollow)
            {
                pfollow.ParentPathLooped = looped;
            }
        }
    }
}
