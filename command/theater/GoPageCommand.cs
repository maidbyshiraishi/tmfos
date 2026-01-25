using Godot;
using tmfos.theater;

namespace tmfos.command.theater;

/// <summary>
/// ストーリーシアターの指定ページへ移動するコマンド
/// </summary>
public partial class GoPageCommand : CommandNode, IStoryTheaterContent
{
    /// <summary>
    /// 移動先のページ
    /// </summary>
    [Export]
    public Control Page { get; set; }

    private StoryTheater _storyTheater = null;

    public override void _Ready()
    {
        AddToGroup(StoryTheater.StoryTheaterContentGroup);
    }

    public void InitializeStoryTheaterContent(StoryTheater storyTheater)
    {
        _storyTheater = storyTheater;
    }

    public override void DoCommand(Node node, bool flag)
    {
        _storyTheater?.GoPage(Page);
    }
}
