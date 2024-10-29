using Godot;

namespace tmfos.system;

/// <summary>
/// SEの制御を行う
/// </summary>
public partial class SePlayer : Node
{
    internal void Play(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        AudioStreamPlayer value = GetNodeOrNull<AudioStreamPlayer>(name);

        if (value is null)
        {
            GD.PrintErr($"効果音{name}が存在しません。");
        }
        else
        {
            value.Play();
        }
    }
}
