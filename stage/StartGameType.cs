namespace tmfos.stage;

/// <summary>
/// ゲーム開始種別
/// </summary>
public enum StartGameType
{
    /// <summary>
    /// 最初から
    /// </summary>
    NewStage,

    /// <summary>
    /// チュートリアル
    /// </summary>
    NewTutorial,

    /// <summary>
    /// ステージ移動
    /// </summary>
    TakeoverStage,

    /// <summary>
    /// ロード
    /// </summary>
    Load,

    /// <summary>
    /// 再開
    /// </summary>
    Restart,

    /// <summary>
    /// 前回データをロード
    /// </summary>
    LoadLastSLot
}
