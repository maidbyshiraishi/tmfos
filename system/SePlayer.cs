using Godot;
using Godot.Collections;

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
            Node node = Lib.GetPackedScene($"res://contents/se/{name}.tscn").Instantiate();

            if (node is not null and AudioStreamPlayer aplayer)
            {
                AddChild(node);
                aplayer.Play();
            }
        }
        else
        {
            value.Play();
        }
    }

    public void ClearAllAudioStreamPlayer()
    {
        Array<Node> nodes = GetChildren();

        foreach (Node node in nodes)
        {
            if (node is AudioStreamPlayer aplayer && !aplayer.Playing)
            {
                _ = aplayer.CallDeferred(Node.MethodName.QueueFree);
            }
        }
    }
}
