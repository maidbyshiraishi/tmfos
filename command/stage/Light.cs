using Godot;
using tmfos.stage;

namespace tmfos.command.stage;

/// <summary>
/// 光源として機能する
/// コマンドとしては何もしない
/// </summary>
public partial class Light : CommandNode2D, ILight
{
    /// <summary>
    /// 照明範囲の倍率
    /// </summary>
    [Export]
    public float RangeRatio { get; set; } = 1f;

    public override void _Ready()
    {
        AddToGroup(StageRoot.LightSourceGroup);
        PointLight2D light = GetNode<PointLight2D>("PointLight2D");
        light.TextureScale = RangeRatio;
    }

    public void EnableLight()
    {
        if (!HasNode("PointLight2D"))
        {
            return;
        }

        PointLight2D pointLight2D = GetNode<PointLight2D>("PointLight2D");
        pointLight2D.Show();
    }
}
