namespace tmfos.mob;

/// <summary>
/// ゲーム内で使用されるオブジェクトの初期化のインタフェース
/// </summary>
public interface IGameNode
{
    /// <summary>
    /// 初期化
    /// </summary>
    void InitializeNode();

    /// <summary>
    /// 終了
    /// </summary>
    void FinalizeNode();

    /// <summary>
    /// 外部要因による除去
    /// </summary>
    void RemoveNode();
}
