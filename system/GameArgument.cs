using Godot;
using System.Collections.Generic;

namespace tmfos.system;

/// <summary>
/// 引数制御
/// </summary>
public partial class GameArgument : Node
{
    private readonly Dictionary<string, Variant[]> dictinary = [];

    public void SetArgument(string key, Variant[] argument)
    {
        if (!dictinary.TryAdd(key, argument))
        {
            dictinary[key] = argument;
        }
    }

    public Variant[] GetArgument(string key)
    {
        if (dictinary.ContainsKey(key))
        {
            _ = dictinary.Remove(key, out Variant[] argument);
            return argument;
        }

        return [];
    }
}
