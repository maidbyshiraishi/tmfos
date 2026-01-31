using Godot;

namespace tmfos.command;

/// <summary>
/// ボタンを押下するコマンド
/// </summary>
public partial class ButtonPressedCommand : CommandRoot
{
    /// <summary>
    /// 押下するボタン
    /// </summary>
    [Export]
    public BaseButton Target { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (Target is not null)
        {
            _ = Target.EmitSignal(BaseButton.SignalName.Pressed);
        }
    }
}
