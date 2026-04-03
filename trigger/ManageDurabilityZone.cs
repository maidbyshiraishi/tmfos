using Godot;
using maid_by_shiraishi.command.stage;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// 耐久力変更ゾーン
/// </summary>
public partial class ManageDurabilityZone : TriggerArea2D
{
    /// <summary>
    /// 耐久力の増減
    /// </summary>
    [Export]
    public int Value { get; set; } = 20;

    public override void _Ready()
    {
        base._Ready();
        AddDurabilityCommand addDurabilityCommand = GetNode<AddDurabilityCommand>("AddDurabilityCommand");
        addDurabilityCommand.Value = Value;
    }
}
