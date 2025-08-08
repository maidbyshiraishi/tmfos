using Godot;
using Godot.Collections;
using tmfos.system;

namespace tmfos.screen;

/// <summary>
/// ライセンス表示ダイアログ
/// </summary>
public partial class LicenseDialog : DialogRoot
{
    private Dictionary _licenseDictionary;
    private TextEdit _componentName;
    private OptionButton _partsList;
    private TextEdit _files;
    private TextEdit _license;
    private OptionButton _licenseList;
    private TextEdit _licenseText;
    private TextEdit _copyright;
    private string _name;
    private Dictionary<TextEdit, string> _text = [];

    protected override void InitializeNode()
    {
        _componentName = GetNode<TextEdit>("Control/ComponentName");
        _partsList = GetNode<OptionButton>("Control/Parts");
        _files = GetNode<TextEdit>("Control/Files");
        _files.GetVScrollBar().Rounded = true;
        _license = GetNode<TextEdit>("Control/License");
        _license.GetVScrollBar().Rounded = true;
        _licenseList = GetNode<OptionButton>("Control/LicenseList");
        _licenseText = GetNode<TextEdit>("Control/LicenseText");
        _licenseText.GetVScrollBar().Rounded = true;
        _copyright = GetNode<TextEdit>("Control/Copyright");
        _copyright.GetVScrollBar().Rounded = true;
        _licenseDictionary = GetLicenceDictinary();
        ShowLicense();
    }

    public override void GetArgument()
    {
        GetGameArgument("LicenseDialog");

        if (m_argument is not null && m_argument.Length == 1 && m_argument[0].VariantType is Variant.Type.String)
        {
            _name = m_argument[0].AsString();
        }
    }

    private Dictionary GetLicenceDictinary()
    {
        Dictionary dic = new()
        {
            { "Godot Engine", Engine.GetLicenseText() },
            { "M+ FONT LICENSE", "These fonts are free software.\nUnlimited permission is granted to use, copy, and distribute it, with or without modification, either commercially and noncommercially.\nTHESE FONTS ARE PROVIDED \"AS IS\" WITHOUT WARRANTY.\n\nこれらのフォントはフリー（自由な）ソフトウエアです。\nあらゆる改変の有無に関わらず、また商業的な利用であっても、自由にご利用、複製、再配布することができますが、全て無保証とさせていただきます。" },
            { "VOICEVOX　四国めたん", "許諾内容\n\n１、商用、非商用ともにご利用いただけます。\n\n２、利用の際にはクレジット表記が必要になります。\n例： VOICEVOX:ずんだもん、VOICEVOX:四国めたん\n\n動画サイトの場合は説明画面や動画内のクレジットなど、ユーザーが気になって見にいった際にはわかる程度のところに記載をお願いします。\nアプリなどでの利用の場合は、アプリの紹介画面などに記載をお願いします。（少し探せばわかる場所に）" },
            { "宇宙から来たメイド", "常識の範囲でお使いください。" }
        };

        dic.Merge(Engine.GetLicenseInfo());
        return dic;
    }

    private void SetTextEdit(TextEdit tedit, string text)
    {
        tedit.SetDeferred("text", text);

        if (_text.ContainsKey(tedit))
        {
            _text.Add(tedit, text);
        }
    }

    public void RepairText()
    {
        foreach (TextEdit tedit in _text.Keys)
        {
            tedit.SetDeferred("text", _text[tedit]);
        }
    }

    private Dictionary GetCopyrightInfo(string name)
    {
        Array<Dictionary> info = Engine.GetCopyrightInfo();

        foreach (Dictionary copyright in info)
        {
            if (copyright.ContainsKey("name") && copyright["name"].VariantType is Variant.Type.String && name == copyright["name"].AsString())
            {
                return copyright;
            }
        }

        return [];
    }

    private void ShowTargetFiles(Dictionary target)
    {
        if (target.ContainsKey("files") && target["files"].VariantType is Variant.Type.Array)
        {
            Array files = target["files"].AsGodotArray();
            SetTextEdit(_files, Lib.JoinString(files));
        }
        else
        {
            _files.Text = "";
        }
    }

    private void ShowTargetLicense(Dictionary target)
    {
        if (_name is "Godot Engine")
        {
        }
        else if (target.ContainsKey("license") && target["license"].VariantType is Variant.Type.String)
        {
            string license = target["license"].AsString();
            SetTextEdit(_license, license);
            _licenseList.Clear();

            if (_licenseDictionary.ContainsKey(license))
            {
                _licenseList.Disabled = false;
                _licenseList.AddItem(license);
                SelectLicense(0);
            }
            else
            {
                string[] del = [" and ", " or "];
                string[] sub_names = license.Split(del, System.StringSplitOptions.None);

                if (sub_names.Length > 0)
                {
                    _licenseList.Disabled = false;

                    foreach (string sub_name in sub_names)
                    {
                        string item = sub_name.Trim();

                        if (!string.IsNullOrEmpty(item))
                        {
                            _licenseList.AddItem(item);
                        }
                    }

                    SelectLicense(0);
                }
            }
        }
        else
        {
            _license.Text = "";
            _licenseList.Disabled = true;
            _licenseList.Clear();
            _licenseText.Text = "";
        }
    }

    private void ShowTargetCopyright(Dictionary target)
    {
        if (target.ContainsKey("copyright") && target["copyright"].VariantType is Variant.Type.Array)
        {
            Array copyrights = target["copyright"].AsGodotArray();
            SetTextEdit(_copyright, Lib.JoinString(copyrights));
        }
        else
        {
            _copyright.Text = "";
        }
    }

    private void ShowLicense()
    {
        SetTextEdit(_componentName, _name);

        switch (_name)
        {
            case "Godot Engine":

                _license.Text = "";
                _licenseList.Disabled = true;
                _licenseList.Clear();
                SetTextEdit(_licenseText, _licenseDictionary["Godot Engine"].AsString());
                break;

            case "PixelMplus":

                _partsList.Clear();
                _partsList.Disabled = true;
                _files.Text = "PixelMplus10-Bold.ttf\nPixelMplus10-Regular.ttf\nPixelMplus12-Bold.ttf\nPixelMplus12-Regular.ttf";
                _license.Text = "M+ FONT LICENSE";
                _licenseList.Disabled = true;
                _licenseList.Clear();
                SetTextEdit(_licenseText, _licenseDictionary["M+ FONT LICENSE"].AsString());
                _copyright.Text = "";
                return;

            case "VOICEVOX　四国めたん":

                _partsList.Clear();
                _partsList.Disabled = true;
                _files.Text = "";
                _license.Text = "";
                _licenseList.Disabled = true;
                _licenseList.Clear();
                SetTextEdit(_licenseText, _licenseDictionary["VOICEVOX　四国めたん"].AsString());
                _copyright.Text = "VOICEVOX:四国めたん";
                return;

            case "宇宙から来たメイド":

                _partsList.Clear();
                _partsList.Disabled = true;
                _files.Text = "";
                _license.Text = "";
                _licenseList.Disabled = true;
                _licenseList.Clear();
                SetTextEdit(_licenseText, _licenseDictionary["宇宙から来たメイド"].AsString());
                _copyright.Text = "SHIRAISHI";
                return;
        }

        Array parts = GetParts();

        for (int i = 0; i < parts.Count; i++)
        {
            _partsList.AddItem(i.ToString());
        }

        ShowPart(0);
    }

    private Array GetParts()
    {
        Dictionary copyrightInfo = GetCopyrightInfo(_name);

        if (copyrightInfo.ContainsKey("parts") && copyrightInfo["parts"].VariantType is Variant.Type.Array)
        {
            Array parts = copyrightInfo["parts"].AsGodotArray();
            return parts;
        }

        return [];
    }

    public void ShowPart(int index)
    {
        Array parts = GetParts();
        Dictionary target;

        if (parts.Count == 0)
        {
            _partsList.Disabled = true;
            _ = new Dictionary();
        }
        else
        {
            if (parts[index].VariantType is Variant.Type.Dictionary)
            {
                _partsList.Disabled = false;
                target = parts[index].AsGodotDictionary();
            }
            else
            {
                _partsList.Disabled = true;
                target = [];
            }

            ShowTargetFiles(target);
            ShowTargetLicense(target);
            ShowTargetCopyright(target);
        }
    }
    public void SelectLicense(int index)
    {
        string _name = _licenseList.GetItemText(index);
        string text = _licenseDictionary.ContainsKey(_name) && _licenseDictionary[_name].VariantType is Variant.Type.String ? _licenseDictionary[_name].AsString() : "";
        SetTextEdit(_licenseText, text);
    }

    public override void _Input(InputEvent ievent)
    {
        Control control = GetFocus();

        if (control is TextEdit tedit)
        {
            VScrollBar vscroll = tedit.GetVScrollBar();

            if (vscroll.Value <= vscroll.MinValue && ievent.IsActionPressed("ui_up"))
            {
                _ = tedit.FindPrevValidFocus().CallDeferred(Control.MethodName.GrabFocus);
            }
            else if (vscroll.MaxValue <= vscroll.Value + vscroll.Page && ievent.IsActionPressed("ui_down"))
            {
                _ = tedit.FindNextValidFocus().CallDeferred(Control.MethodName.GrabFocus);
            }
            else if (tedit.Name == "LicenseText" && vscroll.MaxValue <= vscroll.Value + vscroll.Page + 1f && ievent.IsActionPressed("ui_down"))
            {
                _ = tedit.FindNextValidFocus().CallDeferred(Control.MethodName.GrabFocus);
            }
        }
    }
}
