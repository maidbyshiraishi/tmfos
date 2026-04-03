using Godot;
using maid_by_shiraishi.command.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// 虫眼鏡アイテムで出現する？マーク
/// </summary>
public partial class SearchPointTrigger : TriggerArea2D
{
    public override void _Ready()
    {
        base._Ready();
        AreaEntered += ItemSearched;
    }

    public override void SetOpened(bool opened)
    {
        base.SetOpened(opened);
        SwitchOpenOrCloseAnimationCommand switchOpenOrCloseAnimationCommand = GetNode<SwitchOpenOrCloseAnimationCommand>("SwitchOpenOrCloseAnimationCommand");
        switchOpenOrCloseAnimationCommand.Opened = opened;
    }

    public override void Exec(Node2D node)
    {
    }

    public void ItemSearched(Area2D area2d)
    {
        if (Disable || (OneTime && m_opened))
        {
            return;
        }

        if (OneTime)
        {
            m_opened = true;
        }

        Lib.ExecCommands(this, area2d, true);
    }
}
