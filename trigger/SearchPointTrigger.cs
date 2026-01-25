using Godot;
using tmfos.command.stage;
using tmfos.system;

namespace tmfos.trigger;

/// <summary>
/// 虫眼鏡アイテムで出現する？マーク
/// </summary>
public partial class SearchPointTrigger : TriggerArea2D
{
    public override void _Ready()
    {
        base._Ready();
        _ = Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.ItemSearched));
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
