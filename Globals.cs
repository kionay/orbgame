using Godot;
using Godot.Collections;
using System;

namespace Orbgame.Globals;

public partial class Globals : Node
{
    public Dictionary<NodeType, float> NodeScales = new()
    {
        {NodeType.red, 0.324f},
        {NodeType.pink, 0.554f},
        {NodeType.blue, 0.797f},
        {NodeType.orange, 1.000f},
        {NodeType.yellow, 1.473f},
        {NodeType.light_green, 1.797f},
        {NodeType.green, 1.986f},
    };
}

public enum NodeType
{
    red,
    pink,
    blue,
    orange,
    yellow,
    light_green,
    green
}

public static class GlobalExtensions
{
    public static NodeType ToNodeType(this string nodeName) => Enum.Parse<NodeType>(nodeName);

    public static string ToString(this NodeType nodeType) => Enum.GetName(nodeType);
    public static bool IsBiggestNodeType(this NodeType nodeType) => nodeType ==  Enum.GetValues<NodeType>()[^1];
}