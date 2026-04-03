using Godot;
using Godot.Collections;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.player;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.enemy;

/// <summary>
/// 触手の基点
/// </summary>
public partial class TentacleRoot : Node2D, IGameNode
{
    [Export]
    public bool AutoConnectTentacle = true;

    private Player _player;

    public override void _Ready() => AddToGroup(GameStageRoot.GameNodeGroup);

    private void ConnectTentacles()
    {
        Array<Node> children = GetChildren();
        int length = children.Count;
        bool firstNeck = true;
        TentacleNeck previous = null;

        for (int i = 0; i < length; i++)
        {
            if (children[i] is not TentacleNeck neck)
            {
                continue;
            }

            if (firstNeck)
            {
                firstNeck = false;
                neck.MobPrevious = this;
                previous = neck;
                continue;
            }

            previous.MobSubsequent = neck;
            neck.MobPrevious = previous;
            previous = neck;
        }

        TentacleHead head = GetNode<TentacleHead>("TentacleHead");
        previous.MobSubsequent = head;
        head.MobPrevious = previous;
        head.Root = this;
    }

    public virtual void InitializeNode()
    {
        GameStageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        _player = stageRoot.GetNode<Player>("%Player");

        if (AutoConnectTentacle)
        {
            ConnectTentacles();
        }
    }

    public virtual void FinalizeNode()
    {
    }

    public virtual void RemoveNode()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
    }
}

