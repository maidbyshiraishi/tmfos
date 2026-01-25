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
        _ = GetNode<Button>("Control/Up").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.UpSet));
        _ = GetNode<Button>("Control/Down").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.DownSet));
        _ = GetNode<Button>("Control/Left").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.LeftSet));
        _ = GetNode<Button>("Control/Right").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.RightSet));
        _ = GetNode<Button>("Control/A").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.ASet));
        _ = GetNode<Button>("Control/B").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.BSet));
        _ = GetNode<Button>("Control/Pause").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.PauseSet));
        _ = GetNode<Button>("Control/Option").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.OptionSet));
        _ = GetNode<Button>("Control/Screenshot").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.ScreenshotSet));
        _ = GetNode<Button>("Control/Help").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.HelpSet));
        _ = GetNode<Button>("Control/Swap").Connect(Control.SignalName.FocusEntered, new(this, KeyDialogRoot.MethodName.SwapInfo));
        _ = GetNode<Button>("Control/Swap").Connect(Control.SignalName.MouseEntered, new(this, KeyDialogRoot.MethodName.SwapInfo));
        _ = GetNode<Button>("Control/Swap").Connect(BaseButton.SignalName.Pressed, new(this, KeyDialogRoot.MethodName.SwapAB));
    }
}
