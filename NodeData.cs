using Godot;
using Godot.Collections;

namespace Orbgame;
public partial class NodeData: GodotObject
{
    public float Scale {get;set;}
    public float MergeScore {get;set;}
    public string MergeSoundResourcePath {get;set;}
    public NodeData(Dictionary variantTable)
    {
        Scale = (float)variantTable["scale"].AsDouble();
        MergeScore = (float)variantTable["mergeScore"].AsDouble();
        MergeSoundResourcePath = variantTable["MergeSoundResourcePath"].AsString();
    }
}