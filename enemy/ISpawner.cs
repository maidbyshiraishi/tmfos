using System;

namespace maid_by_shiraishi.enemy;

/// <summary>
/// 敵を生成する機能のインタフェース
/// </summary>
public interface ISpawner
{
    void SetSpawned();

    void ResetSpawned();

    Action GetSignalMethod();
}
