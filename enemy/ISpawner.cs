using Godot;

namespace tmfos.enemy;

/// <summary>
/// 敵を生成する機能のインタフェース
/// </summary>
public interface ISpawner
{
    void SetSpawned();

    void ResetSpawned();

    Callable GetSignalMethod();
}
