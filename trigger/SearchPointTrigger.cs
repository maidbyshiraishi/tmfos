using tmfos.command;

namespace tmfos.trigger;

/// <summary>
/// 虫眼鏡アイテムで出現する？マーク
/// </summary>
public partial class SearchPointTrigger : Trigger2D
{
    public override void SetOpened(bool opened)
    {
        base.SetOpened(opened);
        SwitchOpenOrCloseAnimationCommand switchOpenOrCloseAnimationCommand = GetNode<SwitchOpenOrCloseAnimationCommand>("SwitchOpenOrCloseAnimationCommand");
        switchOpenOrCloseAnimationCommand.Opened = opened;
    }
}
