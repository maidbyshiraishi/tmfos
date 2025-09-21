using Godot;
using tmfos.mob;
using tmfos.stage;
using tmfos.system;

namespace tmfos.hud;

/// <summary>
/// ステータス表示
/// </summary>
public partial class Hud : CanvasLayer, IGameNode
{
    [Export]
    public string Text1 { get; set; }

    [Export]
    public string Text2 { get; set; }

    [Export]
    public string Text3 { get; set; } = "Start!!";

    [Export]
    public bool BossMode { get; set; } = false;

    private Label _time;
    private Label _score;
    private Label _remain;
    private TextureProgressBar _life;

    public override void _Ready()
    {
        AddToGroup(StageRoot.GameNodeGroup);
        _time = GetNodeOrNull<Label>("Time");
        _time.Hide();
        _score = GetNodeOrNull<Label>("Score");
        _score.Hide();
        _remain = GetNodeOrNull<Label>("Remain");
        _remain.Hide();
        _life = GetNodeOrNull<TextureProgressBar>("Life");
        _life.Hide();
    }

    public void InitializeNode()
    {
        GetNode<Label>("Start/Label1").Text = Text1;
        GetNode<Label>("Start/Label2").Text = Text2;
        GetNode<Label>("Start/Label3").Text = Text3;
        GetNode<Control>("Start").Show();
    }

    public void FinalizeNode()
    {
    }

    public void RemoveNode()
    {
    }

    public void UpdateHudLife(int life)
    {
        if (_life is null)
        {
            return;
        }

        if (!_life.Visible)
        {
            _life.Show();
        }

        _life.Value = life;
    }

    public void UpdateHudRemain(int remain)
    {
        Lib.UpdateLabel(_remain, string.Format("{0:D2}", remain));
    }

    public void UpdateHudScore(int score)
    {
        if (BossMode)
        {
            return;
        }

        Lib.UpdateLabel(_score, string.Format("{0:D10}", score));
    }

    public void UpdateHudTime(double time)
    {
        Lib.UpdateLabel(_time, string.Format("{0:#}", time));
    }
}
