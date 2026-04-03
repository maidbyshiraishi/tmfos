using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.screen;

/// <summary>
/// ジュークボックスダイアログ
/// </summary>
public partial class JukeboxDialog : DialogRoot
{
    /// <summary>
    /// BGM音量
    /// </summary>
    [Export]
    public float BgmVolume { get; set; } = 100f;

    public override void _Ready()
    {
        base._Ready();
        GetNode<HSlider>("Control/BgmSlider").ValueChanged += BgmVolumeChanged;
    }

    public override void Active()
    {
        UpdateDialogScreen();
        base.Active();
    }

    protected override string GetDefaultFocusNodeName() => "Back";

    /// <summary>
    /// GUI設定値を更新する
    /// </summary>
    public override void UpdateDialogScreen()
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        BgmVolume = option.BgmVolume;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GetNode<HSlider>("Control/BgmSlider").SetValueNoSignal(BgmVolume);
    }

    /// <summary>
    /// BGM音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void BgmVolumeChanged(double value)
    {
        BgmVolume = (float)value;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.BgmVolume = BgmVolume;
        option.SetOptions();
    }
}
