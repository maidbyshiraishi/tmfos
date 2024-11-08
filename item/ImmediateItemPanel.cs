using tmfos.command;
using tmfos.trigger;

namespace tmfos.item;

/// <summary>
/// その場で直接効果を及ぼすアイテムの親
/// </summary>
public partial class ImmediateItemPanel : TriggerArea2D
{
    public override void SetOpened(bool opened)
    {
        base.SetOpened(opened);
        SwitchOpenOrCloseAnimationCommand switchOpenOrCloseAnimationCommand = GetNode<SwitchOpenOrCloseAnimationCommand>("SwitchOpenOrCloseAnimationCommand");
        switchOpenOrCloseAnimationCommand.Opened = opened;
    }
}
