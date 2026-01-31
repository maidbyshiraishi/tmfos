using Godot;
using Godot.Collections;
using tmfos.system;

namespace tmfos.command;

/// <summary>
/// 条件式コマンド
/// </summary>
public partial class ConditionalCommand : CommandRoot
{
    /// <summary>
    /// 条件式
    /// </summary>
    [Export]
    public string ConditionalExpression { get; set; }

    public override void DoCommand(Node node, bool flag)
    {
        if (Calc())
        {
            Lib.ExecCommands(this, node, true);
        }
        else
        {
            Lib.ExecCommands(this, node, false);
        }
    }

    protected bool Calc()
    {
        try
        {
            // 変数はGameDataのキーと値を使用する
            GetNode<GameData>("/root/GameData").GetKeysAndValues(out string[] keys, out Array values);
            // 式を評価する
            Expression exp = new();

            if (exp.Parse(ConditionalExpression, keys) is not Error.Ok)
            {
                return false;
            }

            Variant variant = exp.Execute(values, null, false);
            return variant.VariantType is Variant.Type.Bool && variant.AsBool();
        }
        catch
        {
        }

        return false;
    }
}
