using Godot;
using Godot.Collections;
using System;

namespace Orbgame.Globals;

public partial class Globals : Node
{
    public float Score {
        get {
            return _score;
        }
        set {
            _score = value;
            GetNode<Label>("/root/Game/ScoreGroup/ScoreValue").Text = $"{_score:n0}";
        }

    }
    private float _score = 0f;
    public static Dictionary<NodeType, float> NodeScales = new()
    {
        {NodeType.red, 0.324f},
        {NodeType.pink, 0.554f},
        {NodeType.blue, 0.797f},
        {NodeType.orange, 1.000f},
        {NodeType.yellow, 1.473f},
        {NodeType.light_green, 1.797f},
        {NodeType.green, 1.986f},
    };

    public static Dictionary<NodeType, float> NodeMergeScore = new()
    {
        {NodeType.red, 1f},
        {NodeType.pink, 11f},
        {NodeType.blue, 21},
        {NodeType.orange, 32f},
        {NodeType.yellow, 41f},
        {NodeType.light_green, 55f},
        {NodeType.green, 66f},
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

    public static CompressedTexture2D GetTexture(this NodeType nodeType) => ResourceLoader.Load<CompressedTexture2D>($"res://Sprites/{nodeType}.svg");
}