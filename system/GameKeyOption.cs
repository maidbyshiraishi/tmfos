using Godot;
using Godot.Collections;

namespace tmfos.system;

/// <summary>
/// キー設定
/// </summary>
public partial class GameKeyOption : Node
{
    public static readonly Array<string> Actions = ["ui_accept", "ui_up", "ui_down", "ui_left", "ui_right", "b", "pause", "option", "help", "screenshot"];

    // データ保存先
    private static readonly string KeyOptionFilePath = "user://key_options.dat";
    private static readonly Array<Key> UnchangeableKeys = [Key.Enter, Key.KpEnter, Key.Kp8, Key.Kp2, Key.Kp4, Key.Kp6, Key.Up, Key.Down, Key.Left, Key.Right, Key.Escape, Key.F1];
    private static readonly Array<JoyButton> UnchangeableJoyButton = [JoyButton.DpadUp, JoyButton.DpadDown, JoyButton.DpadLeft, JoyButton.DpadRight];

    public override void _Ready()
    {
        LoadKeyOptions();
    }

    public static void ResetDefaultKeyOptions()
    {
        InputMap.LoadFromProjectSettings();
    }

    public void LoadKeyOptions()
    {
        ConfigFile keyOptions = new();
        Error e = keyOptions.Load(KeyOptionFilePath);

        if (e is Error.FileNotFound)
        {
            ResetDefaultKeyOptions();
            return;
        }
        else if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{KeyOptionFilePath}を読み込めませんでした。エラーコードは{e}です。ゲームのデフォルト値を使用します。");
            ResetDefaultKeyOptions();
            return;
        }

        foreach (string actionName in Actions)
        {
            if (!Actions.Contains(actionName))
            {
                continue;
            }

            SetUnchangeableKey(actionName);

            if (!keyOptions.HasSection("KeyOption") || !keyOptions.HasSectionKey("KeyOption", actionName))
            {
                ResetDefaultKeyOptions();
                return;
            }

            Variant data = keyOptions.GetValue("KeyOption", actionName);

            try
            {
                string str = data.ToString();
                Array<InputEvent> keys = (Array<InputEvent>)GD.StrToVar(str);
                // StrToVarしなくてもよい。リリースしてしまったので変更しない。
                // Array<InputEvent> keys = data.AsGodotArray<InputEvent>();

                foreach (InputEvent key in keys)
                {
                    if (CanChangeInputEvent(key))
                    {
                        InputMap.ActionAddEvent(actionName, key);
                    }
                }
            }
            catch
            {
            }
        }

        CopyUISelect();
    }

    /// <summary>
    /// ui_acceptをui_selectにコピーする
    /// </summary>
    public void CopyUISelect()
    {
        InputMap.EraseAction("ui_select");
        InputMap.AddAction("ui_select");
        Array<InputEvent> events = InputMap.ActionGetEvents("ui_accept");
        AddAllAction("ui_select", events);
    }

    public void SaveKeyOptions()
    {
        ConfigFile keyOptions = new();
        Array<StringName> actions = InputMap.GetActions();

        foreach (string actionName in actions)
        {
            if (!Actions.Contains(actionName))
            {
                continue;
            }

            Array<InputEvent> events = InputMap.ActionGetEvents(actionName);
            Array<InputEvent> data = [];

            foreach (InputEvent ievent in events)
            {
                if (CanChangeInputEvent(ievent))
                {
                    data.Add(ievent);
                }
            }

            keyOptions.SetValue("KeyOption", actionName, GD.VarToStr(data));
            // VarToStrしなくてもよい。リリースしてしまったので変更しない。
            // keyOptions.SetValue("KeyOption", actionName, data);
        }

        Error e = keyOptions.Save(KeyOptionFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"キー設定の保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
        }
    }

    public static bool CanChangeInputEvent(InputEvent key)
    {
        if (key is InputEventKey ikey)
        {
            return !UnchangeableKeys.Contains(ikey.Keycode);
        }
        else if (key is InputEventJoypadButton ibutton)
        {
            return !UnchangeableJoyButton.Contains(ibutton.ButtonIndex);
        }

        return true;
    }

    private InputEventKey GetInputEventKey(Key key, bool physical = false)
    {
        InputEventKey ievent = new();

        if (physical)
        {
            ievent.PhysicalKeycode = key;
        }
        else
        {
            ievent.Keycode = key;
        }

        return ievent;
    }

    public void SetUnchangeableKey(string actionName)
    {
        switch (actionName)
        {
            case "ui_accept":

                InputMap.EraseAction("ui_accept");
                InputMap.AddAction("ui_accept");
                InputMap.ActionAddEvent("ui_accept", GetInputEventKey(Key.Enter));
                InputMap.ActionAddEvent("ui_accept", GetInputEventKey(Key.KpEnter));
                break;

            case "ui_up":

                InputMap.EraseAction("ui_up");
                InputMap.AddAction("ui_up");
                InputMap.ActionAddEvent("ui_up", GetInputEventKey(Key.Up));
                InputMap.ActionAddEvent("ui_up", GetInputEventKey(Key.Kp8));
                break;

            case "ui_down":

                InputMap.EraseAction("ui_down");
                InputMap.AddAction("ui_down");
                InputMap.ActionAddEvent("ui_down", GetInputEventKey(Key.Down));
                InputMap.ActionAddEvent("ui_down", GetInputEventKey(Key.Kp2));
                break;

            case "ui_left":

                InputMap.EraseAction("ui_left");
                InputMap.AddAction("ui_left");
                InputMap.ActionAddEvent("ui_left", GetInputEventKey(Key.Left));
                InputMap.ActionAddEvent("ui_left", GetInputEventKey(Key.Kp4));
                break;

            case "ui_right":

                InputMap.EraseAction("ui_right");
                InputMap.AddAction("ui_right");
                InputMap.ActionAddEvent("ui_right", GetInputEventKey(Key.Right));
                InputMap.ActionAddEvent("ui_right", GetInputEventKey(Key.Kp6));
                break;

            default:

                InputMap.EraseAction(actionName);
                InputMap.AddAction(actionName);
                break;
        }
    }

    public static Array<InputEvent> GetInputEvent(string actionName)
    {
        Array<StringName> actions = InputMap.GetActions();
        return actions.Contains(actionName) ? InputMap.ActionGetEvents(actionName) : [];
    }

    public static bool CheckAlreadyInUse(InputEvent ievent)
    {
        foreach (StringName actionName in Actions)
        {
            Array<InputEvent> events = InputMap.ActionGetEvents(actionName);

            foreach (InputEvent ievent2 in events)
            {
                switch (ievent)
                {
                    case InputEventJoypadButton jbtn when ievent2 is InputEventJoypadButton jbtn2:

                        if (GD.VarToStr(jbtn) == GD.VarToStr(jbtn2))
                        {
                            return true;
                        }

                        break;

                    case InputEventJoypadMotion jmot when ievent2 is InputEventJoypadMotion jmot2:

                        if (GD.VarToStr(jmot) == GD.VarToStr(jmot2))
                        {
                            return true;
                        }

                        break;

                    case InputEventKey ikey when ievent2 is InputEventKey ikey2:

                        if (ikey.PhysicalKeycode == ikey2.PhysicalKeycode)
                        {
                            return true;
                        }

                        break;
                }
            }
        }

        return false;
    }

    public void SwapAB()
    {
        Array<InputEvent> acceptKeys = InputMap.ActionGetEvents("ui_accept");
        Array<InputEvent> reserveAcceptKeys = [];
        Array<InputEvent> bKeys = InputMap.ActionGetEvents("b");
        Array<InputEvent> reserveBKeys = [];

        foreach (InputEvent item in acceptKeys)
        {
            if (!CanChangeInputEvent(item))
            {
                reserveAcceptKeys.Add(item);
            }
        }

        foreach (InputEvent item in reserveAcceptKeys)
        {
            _ = acceptKeys.Remove(item);
        }

        foreach (InputEvent item in bKeys)
        {
            if (!CanChangeInputEvent(item))
            {
                reserveBKeys.Add(item);
            }
        }

        foreach (InputEvent item in reserveBKeys)
        {
            _ = bKeys.Remove(item);
        }

        InputMap.EraseAction("b");
        InputMap.AddAction("b");

        foreach (InputEvent item in reserveBKeys)
        {
            acceptKeys.Add(item);
        }

        AddAllAction("b", acceptKeys);
        InputMap.EraseAction("ui_accept");
        InputMap.AddAction("ui_accept");

        foreach (InputEvent item in reserveAcceptKeys)
        {
            bKeys.Add(item);
        }

        AddAllAction("ui_accept", bKeys);
        CopyUISelect();
    }

    private void AddAllAction(string actionName, Array<InputEvent> actions)
    {
        foreach (InputEvent action in actions)
        {
            InputMap.ActionAddEvent(actionName, action);
        }
    }
}
