using Godot;
using Godot.Collections;
using tmfos.command;
using tmfos.screen;

namespace tmfos.theater;

/// <summary>
/// ストーリーシアターダイアログ
/// </summary>
public partial class StoryTheater : DialogRoot
{
    public static readonly string StoryTheaterContentGroup = "StoryTheaterContent";

    // 文字を修飾したい場合はBBCode機能を有効にする
    // https://docs.godotengine.org/ja/4.x/tutorials/ui/bbcode_in_richtextlabel.html
    // [center]中央揃え[/center]
    // [color=red]文字色[/color]
    // [wave]ウェーブ[/wave]
    // [tornado]トルネード[/tornado]
    // [shake]シェイク[/shake]
    // [fade start=0 length=9]フェード[/fade]
    // [rainbow]ゲーミング[/rainbow]
    // [outline_size=5]縁取り[/outline_size]
    // [outline_color=red]縁取り色[/outline_color]

    private Control _content;
    private Control _socket;
    private int _index = 0;

    public override void _Ready()
    {
        base._Ready();
        GetTree().CallGroup(StoryTheaterContentGroup, "InitializeStoryTheaterContent", this);
    }

    protected override void InitializeNode()
    {
        _socket = GetNode<Control>("Socket");
        _content = _socket.GetNode<Control>("Contents");
        // 一枚目のみを表示状態に切り替える
        CloseAllPages();
        OpenPage(_index);
    }

    /// <summary>
    /// すべてのページを閉じる
    /// </summary>
    private void CloseAllPages()
    {
        int length = _content.GetChildren().Count;

        for (int i = 0; i < length; i++)
        {
            ClosePage(i);
        }
    }

    /// <summary>
    /// 現在のページを閉じる
    /// </summary>
    private void ClosePage(int index)
    {
        Control nowNode = _content.GetChildOrNull<Control>(index);

        if (nowNode is null)
        {
            return;
        }

        ChangeFocusMode(false, nowNode);
        nowNode.Hide();
    }

    private void OpenPage(int index)
    {
        Control nextNode = _content.GetChildOrNull<Control>(index);

        if (nextNode is null)
        {
            return;
        }

        ChangeFocusMode(true, nextNode);
        nextNode.Show();
        SetFocusFirst(nextNode);
        _index = index;
    }

    private void OpenPage(Control control)
    {
        Array<Node> nodes = _content.GetChildren();
        int index = 0;

        foreach (Node n in nodes)
        {
            if (n is Control c && c == control)
            {
                OpenPage(index);
                return;
            }

            index++;
        }
    }

    public void GoPage(Control control)
    {
        ClosePage(_index);
        OpenPage(control);
    }

    /// <summary>
    /// 次のページを表示する
    /// </summary>
    public void GoNextPage()
    {
        ClosePage(_index);
        _index++;
        OpenPage(_index);
    }

    public override void GetArgument()
    {
        GetGameArgument("StoryTheater");
    }

    private void CloseStory()
    {
        CommandContainer container = GetNode<CommandContainer>("CloseCommand");
        container.ExecCommand(this, true);
    }
}
