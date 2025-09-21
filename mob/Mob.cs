using Godot;
using tmfos.stage;
using tmfos.system;

namespace tmfos.mob;

/// <summary>
/// キャラクターの親
/// </summary>
public partial class Mob : CharacterBody2D, IGameNode
{
    [Signal]
    public delegate void NodeSpawnedEventHandler(Node node, Node spawner, Vector2 position, Vector2 direction, double lifeTime);

    protected AnimatedSprite2D m_animatedSprite;
    protected VisibleOnScreenNotifier2D m_visibleOnScreenNotifier2D;

    public override void _Ready()
    {
        AddToGroup(StageRoot.GameNodeGroup);
        m_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        m_visibleOnScreenNotifier2D = GetNodeOrNull<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
    }

    public virtual void InitializeNode()
    {
        StageRoot stageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot();
        _ = Connect(SignalName.NodeSpawned, new(stageRoot, StageRoot.MethodName.SpawnNode));
    }

    public virtual void FinalizeNode()
    {
    }

    public virtual async void RemoveNode()
    {
        SetPhysicsProcess(false);
        GlobalPosition = new(-2000f, -2000f);

        for (int i = 0; i < 5; i++)
        {
            _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        QueueFree();
    }

    protected void PlaySprite(string name = null)
    {
        if (m_animatedSprite is null || m_animatedSprite.SpriteFrames is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            m_animatedSprite.Play();
        }
        else if (m_animatedSprite.SpriteFrames.HasAnimation(name) && (m_animatedSprite.Animation != name || (m_animatedSprite.Animation == name && !m_animatedSprite.IsPlaying())))
        {
            m_animatedSprite.Play(name);
        }
        else if (m_animatedSprite.SpriteFrames.HasAnimation("default") && (m_animatedSprite.Animation != name || (m_animatedSprite.Animation == name && !m_animatedSprite.IsPlaying())))
        {
            m_animatedSprite.Play("default");
        }
    }

    protected void PauseSprite()
    {
        m_animatedSprite.Pause();
    }

    protected virtual void PlaySe(string name)
    {
        if (m_visibleOnScreenNotifier2D is null || m_visibleOnScreenNotifier2D.IsOnScreen())
        {
            GetNode<SePlayer>("/root/SePlayer").Play(name);
        }
    }
}
