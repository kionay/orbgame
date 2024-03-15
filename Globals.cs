using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace Orbgame.Globals;

public partial class Globals : Node
{
    public Dictionary<NodeType, NodeData> NodeConfiguration = new();
    public override void _Ready()
    {
        using var file = FileAccess.Open("res://Orb.config", FileAccess.ModeFlags.Read);
        var configVariant = Json.ParseString(file.GetAsText());
        file.Close();
        var configVariantTable = configVariant.AsGodotDictionary();
        foreach(var item in configVariantTable)
        {
            var nodeTypeKey = item.Key.AsNodeType();
            var nodeDataValue = item.Value.AsNodeData();
            NodeConfiguration[nodeTypeKey] = nodeDataValue;
        }

        base._Ready();
    }
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
    public static NodeType AsNodeType(this Variant variant) => variant.AsString().ToNodeType();
    public static NodeData AsNodeData(this Variant variant) => new(variant.AsGodotDictionary());
    public static bool IsBiggestNodeType(this NodeType nodeType) => nodeType ==  Enum.GetValues<NodeType>()[^1];
    public static CompressedTexture2D GetTexture(this NodeType nodeType) => ResourceLoader.Load<CompressedTexture2D>($"res://Sprites/{nodeType}.svg");
}