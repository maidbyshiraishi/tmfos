namespace tmfos.system;

/// <summary>
/// 画面間で状態を保存するオブジェクトのインタフェース
/// </summary>
public interface IStateful
{
    void StateSave();

    void StateLoad();
}
