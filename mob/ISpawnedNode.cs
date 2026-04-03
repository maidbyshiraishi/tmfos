using Godot;
using maid_by_shiraishi.enemy;

namespace maid_by_shiraishi.mob;

/// <summary>
/// 動的に生成されるノードのインタフェース
/// </summary>
public interface ISpawnedNode
{
    void SetNodeInfo(Vector2 position, Vector2 direction);

    void SetLifeTime(double lifeTime);

    void SetSpawner(ISpawner spawner);
}
