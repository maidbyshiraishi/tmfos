namespace tmfos.others;

/// <summary>
/// パス終端タイプ
/// </summary>
public enum EdgeHandlingType
{
    /// <summary>
    /// 端まで行ったら終わり
    /// </summary>
    Oneway,

    /// <summary>
    /// 往復する
    /// </summary>
    Shuttle,

    /// <summary>
    /// 終端から発端へ戻る
    /// </summary>
    Loop
}
