using Godot;
using tmfos.theater;

namespace tmfos.command;

/// <summary>
/// ストーリーシアターの次ページへ移動するコマンド
/// </summary>
public partial class GoNextPageCommand : CommandNode, IStoryTheaterContent
{
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
        _storyTheater?.GoNextPage();
    }
}
