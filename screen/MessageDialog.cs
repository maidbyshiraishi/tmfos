using Godot;
using maid_by_shiraishi.trigger;

namespace maid_by_shiraishi.screen;

/// <summary>
/// メッセージダイアログ
/// </summary>
public partial class MessageDialog : DialogRoot
{
    public override void GetArgument()
    {
        GetGameArgument("MessageDialog");

        // 引数はメッセージのみ、あるいはメッセージとESCキーの有効・無効の2つ
        if (m_argument is not null && m_argument.Length == 1 && m_argument[0].VariantType is Variant.Type.String)
        {
            SetMessage(m_argument[0].AsString());
        }
        else if (m_argument is not null && m_argument.Length == 2 && m_argument[0].VariantType is Variant.Type.String && m_argument[1].VariantType is Variant.Type.Bool)
        {
            SetMessage(m_argument[0].AsString());

            if (!m_argument[1].AsBool())
            {
                GetNode<KeyReleaseedTrigger>("EscapeKey").ActionName = null;
            }
        }
    }

    public void SetMessage(string message)
    {
        Label textEdit = GetNode<Label>("Message");
        textEdit.Text = message;
    }

    protected override string GetDefaultFocusNodeName() => "Back";
}
