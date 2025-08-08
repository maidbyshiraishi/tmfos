using Godot;

namespace tmfos.others;

/// <summary>
/// 動く床
/// </summary>
public partial class MovingPlatform : PathFollow
{
    private CharacterBody2D _characterBody;

    public override void _Ready()
    {
        base._Ready();
        _characterBody = GetNodeOrNull<CharacterBody2D>("CharacterBody2D");
    }

    protected override void ExecLoop()
    {
        if (_characterBody is null)
        {
            return;
        }

        switch (ProgressRatio)
        {
            case 0.0f:

                _characterBody.SetCollisionLayerValue(2, true);
                ProgressRatio = 1.0f;
                ReachedToEdge();
                break;

            case 1.0f:

                _characterBody.SetCollisionLayerValue(2, true);
                ProgressRatio = 0.0f;
                ReachedToEdge();
                break;

            case < 0.01f when Reverse && ParentPathLooped:

                _characterBody.SetCollisionLayerValue(2, false);
                break;

            case > 0.09f when !Reverse && !ParentPathLooped:

                _characterBody.SetCollisionLayerValue(2, false);
                break;
        }
    }
}
