using Godot;

namespace maid_by_shiraishi.system;

public partial class StopExecTwice : Node
{
    public override void _EnterTree()
    {
        if (!GameMutex.IsExecuted())
        {
            GetTree().Quit();
        }
    }
}
