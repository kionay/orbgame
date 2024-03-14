using Godot;
using Godot.Collections;

namespace Orbgame;
public partial class NodeData: GodotObject
{
    public float Scale {get;set;}
    public float MergeScore {get;set;}
    public float MergePitchScale {get;set;}
    public NodeData(Dictionary variantTable)
    {
        Scale = (float)variantTable["scale"].AsDouble();
        MergeScore = (float)variantTable["mergeScore"].AsDouble();
        MergePitchScale = (float)variantTable["mergePitchScale"].AsDouble();
    }
}