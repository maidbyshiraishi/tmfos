using Godot;
using tmfos.system;

namespace tmfos.screen;

public partial class KeyCommandTestDialog : DialogRoot
{
    /// <summary>
    /// 昇竜拳: →↓↘+P
    /// </summary>
    private int[] shouryu_ken_comm = [0b_0000_0010_0000_0000, 0b_0000_1000_0000_0000, 0b_0000_1010_1000_0000,];
    private int[] shouryu_ken_mask = [0b_0000_1010_1100_1100, 0b_0000_1010_1100_1100, 0b_0000_1010_1100_1100,];
    private int[] shouryu_ken_tame = [0, 0, 0,];

    /// <summary>
    /// 波動拳: ↓↘→+P
    /// </summary>
    private int[] hadou_ken_comm = [0b_0000_1000_0000_0000, 0b_0000_1010_0000_0000, 0b_0000_0010_1000_0000,];
    private int[] hadou_ken_mask = [0b_0000_1000_1100_1100, 0b_0000_1010_1100_1100, 0b_0000_1010_1100_1100,];
    private int[] hadou_ken_tame = [0, 0, 0,];

    /// <summary>
    /// バル・ロゼ: ←タメ→+P
    /// </summary>
    private int[] ballerose_comm = [0b_0000_0100_0000_0000, 0b_0000_0010_1000_0000];
    private int[] ballerose_mask = [0b_0000_0100_1100_1100, 0b_0000_1010_1100_1100];
    private int[] ballerose_tame = [120, 0,];

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (GetNode<GameKeyCommandController>("/root/GameKeyCommandController").FindCommand(shouryu_ken_comm, shouryu_ken_mask, shouryu_ken_tame, false))
        {
            GD.Print("昇竜拳!!");
        }

        if (GetNode<GameKeyCommandController>("/root/GameKeyCommandController").FindCommand(hadou_ken_comm, hadou_ken_mask, hadou_ken_tame, false))
        {
            GD.Print("波動拳!!");
        }

        if (GetNode<GameKeyCommandController>("/root/GameKeyCommandController").FindCommand(ballerose_comm, ballerose_mask, ballerose_tame, false))
        {
            GD.Print("バル・ロゼ!!");
        }
    }
}
