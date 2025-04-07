using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInfo : MonoBehaviour
{
    public int BlendShapeIndex;
    public int MyListIndex;
    public int AffectedVertexInd;
    public AxisType AxisType;
    public Side ObjectSide;
}

public enum AxisType
{
    X_Axis,
    Y_Axis
}

public enum Side
{
    Left,
    Right,
    Middle
}