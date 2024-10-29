using Godot;

namespace tmfos.enemy;

/// <summary>
/// 敵を生成する機能のインタフェース
/// </summary>
public interface ISpawner
{
    public void SetSpawned();

    public void ResetSpawned();

    public Callable GetSignalMethod();
}
