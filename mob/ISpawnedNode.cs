using Godot;
using tmfos.enemy;

namespace tmfos.mob;

/// <summary>
/// 動的に生成されるノードのインタフェース
/// </summary>
public interface ISpawnedNode
{
    public void SetNodeInfo(Vector2 position, Vector2 direction);

    public void SetLifeTime(double lifeTime);

    public void SetSpawner(ISpawner spawner);
}
