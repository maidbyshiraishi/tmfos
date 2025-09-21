namespace tmfos.enemy;

/// <summary>
/// Eyesightによって制御された敵
/// </summary>
public partial class EyesightEnemy : Enemy
{
    protected Eyesight m_eyesight;

    public override void _Ready()
    {
        base._Ready();
        m_eyesight = GetNode<Eyesight>("EyeSight");
    }
}
