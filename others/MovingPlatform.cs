using Godot;

namespace tmfos.others;

/// <summary>
/// 動く床
/// </summary>
public partial class MovingPlatform : PathFollow
{
    [Export]
    public CharacterBody2D CharacterBody { get; set; }

    protected override void ExecLoop()
    {
        if (CharacterBody is null)
        {
            return;
        }

        switch (ProgressRatio)
        {
            case 0.0f:

                CharacterBody.SetCollisionLayerValue(2, true);
                ProgressRatio = 1.0f;
                ReachedToEdge();
                break;

            case 1.0f:

                CharacterBody.SetCollisionLayerValue(2, true);
                ProgressRatio = 0.0f;
                ReachedToEdge();
                break;

            case < 0.01f when Reverse && ParentPathLooped:

                CharacterBody.SetCollisionLayerValue(2, false);
                break;

            case > 0.09f when !Reverse && !ParentPathLooped:

                CharacterBody.SetCollisionLayerValue(2, false);
                break;
        }
    }
}
