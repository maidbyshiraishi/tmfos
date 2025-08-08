using Godot;
using tmfos.enemy;

namespace tmfos.mob;

/// <summary>
/// 動的に生成されるノードのインタフェース
/// </summary>
public interface ISpawnedNode
{
    void SetNodeInfo(Vector2 position, Vector2 direction);

    void SetLifeTime(double lifeTime);

    void SetSpawner(ISpawner spawner);
}
