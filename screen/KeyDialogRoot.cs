using Godot;
using Godot.Collections;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// キー設定ダイアログの親
/// </summary>
public partial class KeyDialogRoot : DialogRoot
{
    private Label _info;
    private Label _keyInfo;
    private bool _setMode = false;
    private string _setActionName;
    private Control _localFocus;
    private string _localInfo;
    private string _localActionName;
    private string _lastLabel;

    public override void _Ready()
    {
        base._Ready();
        _ = GetNode<Button>("Control/Up").Connect(Control.SignalName.FocusEntered, new(this, MethodName.UpInfo));
        _ = GetNode<Button>("Control/Up").Connect(Control.SignalName.MouseEntered, new(this, MethodName.UpInfo));
        _ = GetNode<Button>("Control/Down").Connect(Control.SignalName.FocusEntered, new(this, MethodName.DownInfo));
        _ = GetNode<Button>("Control/Down").Connect(Control.SignalName.MouseEntered, new(this, MethodName.DownInfo));
        _ = GetNode<Button>("Control/Left").Connect(Control.SignalName.FocusEntered, new(this, MethodName.LeftInfo));
        _ = GetNode<Button>("Control/Left").Connect(Control.SignalName.MouseEntered, new(this, MethodName.LeftInfo));
        _ = GetNode<Button>("Control/Right").Connect(Control.SignalName.FocusEntered, new(this, MethodName.RightInfo));
        _ = GetNode<Button>("Control/Right").Connect(Control.SignalName.MouseEntered, new(this, MethodName.RightInfo));
        _ = GetNode<Button>("Control/A").Connect(Control.SignalName.FocusEntered, new(this, MethodName.AInfo));
        _ = GetNode<Button>("Control/A").Connect(Control.SignalName.MouseEntered, new(this, MethodName.AInfo));
        _ = GetNode<Button>("Control/B").Connect(Control.SignalName.FocusEntered, new(this, MethodName.BInfo));
        _ = GetNode<Button>("Control/B").Connect(Control.SignalName.MouseEntered, new(this, MethodName.BInfo));
        _ = GetNode<Button>("Control/Pause").Connect(Control.SignalName.FocusEntered, new(this, MethodName.PauseInfo));
        _ = GetNode<Button>("Control/Pause").Connect(Control.SignalName.MouseEntered, new(this, MethodName.PauseInfo));
        _ = GetNode<Button>("Control/Option").Connect(Control.SignalName.FocusEntered, new(this, MethodName.OptionInfo));
        _ = GetNode<Button>("Control/Option").Connect(Control.SignalName.MouseEntered, new(this, MethodName.OptionInfo));
        _ = GetNode<Button>("Control/Screenshot").Connect(Control.SignalName.FocusEntered, new(this, MethodName.ScreenshotInfo));
        _ = GetNode<Button>("Control/Screenshot").Connect(Control.SignalName.MouseEntered, new(this, MethodName.ScreenshotInfo));
        _ = GetNode<Button>("Control/Help").Connect(Control.SignalName.FocusEntered, new(this, MethodName.HelpInfo));
        _ = GetNode<Button>("Control/Help").Connect(Control.SignalName.MouseEntered, new(this, MethodName.HelpInfo));
    }

    protected override void InitializeNode()
    {
        _info = GetNode<Label>("Info");
        _keyInfo = GetNode<Label>("KeyInfo");
        UpInfo();
    }

    public override void Active()
    {
        base.Active();
        ActiveLastFocus();
    }

    private void ActiveLastFocus()
    {
        if (_localFocus is null)
        {
            return;
        }

        _localFocus.SetBlockSignals(true);
        _info.Text = _localInfo;
        UpdateLabel(_localActionName);
        _localFocus.GrabFocus();
        _localFocus.SetBlockSignals(false);
    }

    private void UpdateLabel(string actionName)
    {
        if (string.IsNullOrEmpty(actionName))
        {
            return;
        }

        _lastLabel = actionName;
        Array<InputEvent> events = GameKeyOption.GetInputEvent(actionName);

        if (events.Count == 0)
        {
            _keyInfo.Text = "定義されたキーはありません。";
            return;
        }

        string text = "";

        foreach (InputEvent e in events)
        {
            text += $", {e.AsText()}";
        }

        text = ReplaceButtonName(text);
        _keyInfo.Text = text[2..];
    }

    private string ReplaceButtonName(string text)
    {
        text = text.Replace("Kp ", "テンキー");
        text = text.Replace(" (Physical)", "");
        text = text.Replace("Up", "上");
        text = text.Replace("Top", "上");
        text = text.Replace("Bottom", "下");
        text = text.Replace("Down", "下");
        text = text.Replace("Left", "左");
        text = text.Replace("Right", "右");
        text = text.Replace("Joypad Button", "ゲームパッドボタン");
        text = text.Replace("Guide", "ガイド");
        text = text.Replace("Sony PS", "Sony PS");
        text = text.Replace("Xbox Home", "XBoxホーム");
        text = text.Replace("Square", "□");
        text = text.Replace("Triangle", "△");
        text = text.Replace("Circle", "〇");
        text = text.Replace("Cross", "×");
        return text;
    }

    private void EnterSetMode(string text, string name)
    {
        if (_setMode)
        {
            return;
        }

        _localFocus = GetFocus();
        _localInfo = _info.Text;
        _localActionName = name;
        ChangeFocusMode(false);
        _info.Text = text;
        _setMode = true;
        _setActionName = name;
    }

    private void ShowInfo(string text)
    {
        if (_setMode)
        {
            return;
        }

        _info.Text = text;
    }

    public void UpSet()
    {
        EnterSetMode("上キーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。上カーソルキーとテンキーの8キーは割り当て固定で変更できません。", "ui_up");
    }

    public void UpInfo()
    {
        ShowInfo("上キーは次の目的で使用されます。メニューの上移動、ハシゴにつかまって登る、水中で上移動、扉に入る、調べる");
        UpdateLabel("ui_up");
    }

    public void DownSet()
    {
        EnterSetMode("下キーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。下カーソルキーとテンキーの2キーは割り当て固定で変更できません。", "ui_down");
    }

    public void DownInfo()
    {
        ShowInfo("下キーは次の目的で使用されます。メニューの下移動、ハシゴにつかまって降りる、水中で下移動、ジャンプキーと同時に押して床から飛び降りる");
        UpdateLabel("ui_down");
    }

    public void LeftSet()
    {
        EnterSetMode("左キーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。左カーソルキーとテンキーの4キーは割り当て固定で変更できません。", "ui_left");
    }

    public void LeftInfo()
    {
        ShowInfo("左キーは次の目的で使用されます。メニューの左移動、左へ移動する、水中で左移動");
        UpdateLabel("ui_left");
    }

    public void RightSet()
    {
        EnterSetMode("右キーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。右カーソルキーとテンキーの6キーは割り当て固定で変更できません。", "ui_right");
    }

    public void RightInfo()
    {
        ShowInfo("右キーは次の目的で使用されます。メニューの右移動、右へ移動する、水中で右移動");
        UpdateLabel("ui_right");
    }

    public void ASet()
    {
        EnterSetMode("Aキーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。エンターキーとテンキーのエンターキーは割り当て固定で変更できません。", "ui_accept");
    }

    public void AInfo()
    {
        ShowInfo("Aキーは次の目的で使用されます。メニューの決定、ジャンプする、下と同時に押して床から飛び降りる、水中で押下中は高速移動");
        UpdateLabel("ui_accept");
    }

    public void BSet()
    {
        EnterSetMode("Bキーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。", "b");
    }

    public void BInfo()
    {
        ShowInfo("Bキーは次の目的で使用されます。攻撃する");
        UpdateLabel("b");
    }

    public void PauseSet()
    {
        EnterSetMode("Pauseキーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。", "pause");
    }

    public void PauseInfo()
    {
        ShowInfo("ポーズキーを押すと、いつでもゲームを一時停止できます。また、所有するアイテムを確認できます。");
        UpdateLabel("pause");
    }

    public void OptionSet()
    {
        EnterSetMode("Optionキーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。", "option");
    }

    public void OptionInfo()
    {
        ShowInfo("オプションキーを押すと、ゲーム中に設定画面を開けます。");
        UpdateLabel("option");
    }

    public void ScreenshotSet()
    {
        EnterSetMode("スクリーンショットキーの割り当てを行います。割り当てたいキーやボタンを押してください。キーボードとゲームパッドは個別に設定できます。", "screenshot");
    }

    public void ScreenshotInfo()
    {
        ShowInfo($"スクリーンショットキーを押すとゲーム画面を保存できます。画像はデータフォルダに保存されます。");
        UpdateLabel("screenshot");
    }

    public void SwapInfo()
    {
        ShowInfo($"AキーとBキーの設定を入れ替えます。");
        _keyInfo.Text = "";
    }

    public void HelpSet()
    {
        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", ["ヘルプキーは変更できません。", false]);
    }

    public void HelpInfo()
    {
        ShowInfo("ゲーム中にヘルプキーを押すと操作説明を見ることができます。");
        UpdateLabel("help");
    }

    private Error SetInputEventKey(string actionName, InputEventKey key)
    {
        Array<string> actions = GameKeyOption.Actions;

        if (!actions.Contains(actionName))
        {
            return Error.InvalidData;
        }

        if (GameKeyOption.CheckAlreadyInUse(key))
        {
            return Error.AlreadyInUse;
        }

        Array<InputEvent> events = InputMap.ActionGetEvents(actionName);
        Array<InputEvent> save = [];

        foreach (InputEvent item in events)
        {
            if (item is InputEventJoypadButton or InputEventJoypadMotion)
            {
                save.Add(item);
            }
        }

        GetNode<GameKeyOption>("/root/GameKeyOption").SetUnchangeableKey(actionName);

        foreach (InputEvent item in save)
        {
            InputMap.ActionAddEvent(actionName, item);
        }

        InputMap.ActionAddEvent(actionName, key);
        return Error.Ok;
    }

    private Error SetInputEventJoypadButton(string actionName, InputEventJoypadButton button)
    {
        Array<string> actions = GameKeyOption.Actions;

        if (!actions.Contains(actionName))
        {
            return Error.InvalidData;
        }

        if (GameKeyOption.CheckAlreadyInUse(button))
        {
            return Error.AlreadyInUse;
        }

        Array<InputEvent> events = InputMap.ActionGetEvents(actionName);
        Array<InputEvent> save = [];

        foreach (InputEvent item in events)
        {
            if (item is InputEventKey)
            {
                save.Add(item);
            }
        }

        GetNode<GameKeyOption>("/root/GameKeyOption").SetUnchangeableKey(actionName);

        foreach (InputEvent item in save)
        {
            InputMap.ActionAddEvent(actionName, item);
        }

        InputMap.ActionAddEvent(actionName, button);
        return Error.Ok;
    }

    private Error SetInputEventJoypadMotion(string actionName, InputEventJoypadMotion motion)
    {
        Array<string> actions = GameKeyOption.Actions;

        if (!actions.Contains(actionName))
        {
            return Error.InvalidData;
        }

        if (GameKeyOption.CheckAlreadyInUse(motion))
        {
            return Error.AlreadyInUse;
        }

        Array<InputEvent> events = InputMap.ActionGetEvents(actionName);
        Array<InputEvent> save = [];

        foreach (InputEvent item in events)
        {
            if (item is InputEventKey)
            {
                save.Add(item);
            }
        }

        GetNode<GameKeyOption>("/root/GameKeyOption").SetUnchangeableKey(actionName);

        foreach (InputEvent item in save)
        {
            InputMap.ActionAddEvent(actionName, item);
        }

        InputMap.ActionAddEvent(actionName, motion);
        return Error.Ok;
    }

    public override void _UnhandledInput(InputEvent ievent)
    {
        if (!_setMode)
        {
            return;
        }

        switch (ievent)
        {
            case InputEventKey key when key.IsPressed():
                {
                    if (GameKeyOption.CanChangeInputEvent(key))
                    {
                        Error result = SetInputEventKey(_setActionName, key);

                        switch (result)
                        {
                            case Error.Ok:

                                GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{key.AsText()}に設定しました。", false]);
                                break;

                            case Error.AlreadyInUse:

                                GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{key.AsText()}は既に使用されています。", false]);
                                break;

                            default:

                                GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{key.AsText()}に変更できませんでした。", false]);
                                break;
                        }
                    }
                    else
                    {
                        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [ReplaceButtonName($"{key.AsText()}は設定できません。"), false]);
                    }

                    ChangeFocusMode(true);
                    _setMode = false;
                    break;
                }

            case InputEventJoypadButton button when button.IsPressed():
                {
                    Error result = SetInputEventJoypadButton(_setActionName, button);

                    switch (result)
                    {
                        case Error.Ok:

                            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{button.AsText()}に設定しました。", false]);
                            break;

                        case Error.AlreadyInUse:

                            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{button.AsText()}は既に使用されています。", false]);
                            break;

                        default:

                            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{button.AsText()}に変更できませんでした。", false]);
                            break;
                    }

                    ChangeFocusMode(true);
                    _setMode = false;
                    break;
                }

            case InputEventJoypadMotion motion when motion.IsPressed() && motion.AxisValue != 0:
                {
                    Error result = SetInputEventJoypadMotion(_setActionName, motion);

                    switch (result)
                    {
                        case Error.Ok:

                            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{motion.AsText()}に設定しました。", false]);
                            break;

                        case Error.AlreadyInUse:

                            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{motion.AsText()}は既に使用されています。", false]);
                            break;

                        default:

                            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/message_dialog.tscn", "MessageDialog", [$"{motion.AsText()}に変更できませんでした。", false]);
                            break;
                    }

                    ChangeFocusMode(true);
                    _setMode = false;
                    break;
                }
        }

        if (_setActionName is "ui_accept")
        {
            GetNode<GameKeyOption>("/root/GameKeyOption").CopyUISelect();
        }
    }

    public override void UpdateDialogScreen()
    {
        UpdateLabel(_lastLabel);
    }

    public void SwapAB()
    {
        GetNode<GameKeyOption>("/root/GameKeyOption").SwapAB();
    }
}
