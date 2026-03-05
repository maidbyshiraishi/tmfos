using Godot;

namespace tmfos.screen;

/// <summary>
/// キー設定ダイアログ
/// </summary>
public partial class KeyOptionDialog : KeyDialogRoot
{
    public override void _Ready()
    {
        base._Ready();
        GetNode<Button>("Control/Up").Pressed += UpSet;
        GetNode<Button>("Control/Down").Pressed += DownSet;
        GetNode<Button>("Control/Left").Pressed += LeftSet;
        GetNode<Button>("Control/Right").Pressed += RightSet;
        GetNode<Button>("Control/A").Pressed += ASet;
        GetNode<Button>("Control/B").Pressed += BSet;
        GetNode<Button>("Control/Pause").Pressed += PauseSet;
        GetNode<Button>("Control/Option").Pressed += OptionSet;
        GetNode<Button>("Control/Screenshot").Pressed += ScreenshotSet;
        GetNode<Button>("Control/Help").Pressed += HelpSet;
        GetNode<Button>("Control/Swap").FocusEntered += SwapInfo;
        GetNode<Button>("Control/Swap").MouseEntered += SwapInfo;
        GetNode<Button>("Control/Swap").Pressed += SwapAB;
    }
}
