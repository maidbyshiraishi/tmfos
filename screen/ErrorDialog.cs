using Godot;

namespace tmfos.screen;

/// <summary>
/// エラーダイアログ
/// </summary>
public partial class ErrorDialog : DialogRoot
{
    /// <summary>
    /// 引数を設定する
    /// </summary>
    /// <param name="argument">引数</param>
    public override void GetArgument()
    {
        GetGameArgument("ErrorDialog");

        if (m_argument is not null && m_argument.Length == 1 && m_argument[0].VariantType is Variant.Type.String)
        {
            SetMessage(m_argument[0].AsString());
        }
    }

    /// <summary>
    /// メッセージを設定する
    /// </summary>
    /// <param name="message">エラーメッセージ</param>
    public void SetMessage(string message)
    {
        TextEdit textEdit = GetNode<TextEdit>("Control/Message");
        textEdit.Text = message;
    }
}
