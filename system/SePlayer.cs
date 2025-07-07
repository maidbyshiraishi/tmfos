using Godot;
using Godot.Collections;

namespace tmfos.system;

/// <summary>
/// SEの制御を行う
/// </summary>
public partial class SePlayer : Node
{
    [Export]
    public Dictionary<string, int> MaxPolyphony { get; set; } = [];

    public void Play(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (GetNodeOrNull(name) is AudioStreamPlayer se && IsInstanceValid(se))
        {
            se.Play();
            return;
        }

        if (Lib.GetPackedScene<AudioStream>($"res://contents/se/{name}.ogg") is not AudioStream audio)
        {
            return;
        }

        AudioStreamPlayer audioStreamPlayer = new()
        {
            Name = name,
            Bus = "SE",
            MaxPolyphony = MaxPolyphony.TryGetValue(name, out int value) ? value : 1,
            Stream = audio
        };

        AddChild(audioStreamPlayer);
        audioStreamPlayer.Play();
    }

    public void ClearAllAudioStreamPlayer()
    {
        Array<Node> nodes = GetChildren();

        foreach (Node node in nodes)
        {
            if (node is AudioStreamPlayer aplayer && !aplayer.Playing)
            {
                aplayer.Name = "remove";
                aplayer.QueueFree();
            }
        }
    }
}
