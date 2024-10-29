using Godot;

namespace tmfos.screen;

/// <summary>
/// メッセージダイアログ
/// </summary>
public partial class MessageDialog : DialogRoot
{
    public override void GetArgument()
    {
        GetGameArgument("MessageDialog");

        if (m_argument is not null && m_argument.Length == 1 && m_argument[0].VariantType is Variant.Type.String)
        {
            SetMessage(m_argument[0].AsString());
        }
    }

    public void SetMessage(string message)
    {
        Label textEdit = GetNode<Label>("Control/Message");
        textEdit.Text = message;
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
