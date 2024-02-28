using Godot;
using System;

namespace Orbgame.Globals;

public partial class Globals : Node
{
    


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
}