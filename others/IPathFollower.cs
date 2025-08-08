namespace tmfos.others;

/// <summary>
/// パス移動時に終端処理を行うインタフェース
/// </summary>
public interface IPathFollower
{
    void ReachedToEdge(float progressRatio, bool reverse, EdgeHandlingType edgeHandling);
}
