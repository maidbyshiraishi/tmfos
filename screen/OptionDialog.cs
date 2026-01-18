using Godot;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// ゲーム設定ダイアログ
/// </summary>
public partial class OptionDialog : DialogRoot
{
    /// <summary>
    /// BGM音量
    /// </summary>
    [Export]
    public float BgmVolume { get; set; } = 100f;

    /// <summary>
    /// 効果音音量
    /// </summary>
    [Export]
    public float SeVolume { get; set; } = 100f;

    /// <summary>
    /// 全画面
    /// </summary>
    [Export]
    public bool Fullscreen { get; set; } = false;

    public override void _Ready()
    {
        base._Ready();
        _ = GetNode<HSlider>("Control/BgmSlider").Connect(Range.SignalName.ValueChanged, new(this, MethodName.BgmVolumeChanged));
        _ = GetNode<HSlider>("Control/SeSlider").Connect(Range.SignalName.ValueChanged, new(this, MethodName.SeVolumeChanged));
        _ = GetNode<CheckButton>("Control/FullscreenCheck").Connect(BaseButton.SignalName.Toggled, new(this, MethodName.FullscreenChanged));
        _ = GetNode<Button>("Control/ResetScreen").Connect(BaseButton.SignalName.Pressed, new(this, MethodName.ResetDefaultScreenOptions));
    }

    public override void Active()
    {
        UpdateDialogScreen();
        base.Active();
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "BgmSlider";
    }

    /// <summary>
    /// GUI設定値を更新する
    /// </summary>
    public override void UpdateDialogScreen()
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        BgmVolume = option.BgmVolume;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GetNode<HSlider>("Control/BgmSlider").SetValueNoSignal(BgmVolume);
        SeVolume = option.SeVolume;
        GetNode<Label>("SeValue").Text = $"{SeVolume}%";
        GetNode<HSlider>("Control/SeSlider").SetValueNoSignal(SeVolume);
        GameScreenOption screenOption = GetNode<GameScreenOption>("/root/GameScreenOption");
        Fullscreen = screenOption.Fullscreen;
        SetFullscreenCheck(Fullscreen);
    }

    /// <summary>
    /// BGM音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void BgmVolumeChanged(float value)
    {
        BgmVolume = value;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.BgmVolume = BgmVolume;
        option.SetOptions();
    }

    /// <summary>
    /// 効果音音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void SeVolumeChanged(float value)
    {
        SeVolume = value;
        GetNode<Label>("SeValue").Text = $"{SeVolume}%";
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.SeVolume = SeVolume;
        option.SetOptions();
        GetNode<SePlayer>("/root/SePlayer").Play("menu_select");
    }

    /// <summary>
    /// ウィンドウ・フルスクリーンが切り替わった
    /// </summary>
    /// <param name="toggledOn">ウィンドウ状態</param>
    public void FullscreenChanged(bool toggledOn)
    {
        Fullscreen = toggledOn;
        GetNode<Label>("FullscreenCheckValue").Text = Fullscreen ? "ON" : "OFF";

        // ウィンドウ状態に関しては即座にシステムに反映する
        GameScreenOption option = GetNode<GameScreenOption>("/root/GameScreenOption");
        option.Fullscreen = Fullscreen;
        option.ChangeWindowMode();
    }

    private void SetFullscreenCheck(bool flag)
    {
        GetNode<Label>("FullscreenCheckValue").Text = flag ? "ON" : "OFF";
        GetNode<CheckButton>("Control/FullscreenCheck").SetPressedNoSignal(flag);
    }

    public void ResetDefaultScreenOptions()
    {
        GameScreenOption option = GetNode<GameScreenOption>("/root/GameScreenOption");
        option.CalcScreenOptions();
        option.ApplyScreenOptions();
    }
}
