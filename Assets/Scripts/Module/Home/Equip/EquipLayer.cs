using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 装备界面
/// </summary>
public class EquipLayer : UILayer
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "EquipLayer"; }
    }

    protected override void OnEnter()
    {
        Shape shape = view.GetChild("graph_line").asGraph.shape;
        LineMesh line2 = shape.graphics.GetMeshFactory<LineMesh>();
        line2.lineWidth = 3;
        line2.roundEdge = true;
        line2.path.Create(new GPathPoint[] {
            new GPathPoint(new Vector3(0, 120, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(60, 30, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(80, 90, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(140, 30, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(160, 90, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(220, 30, 0), GPathPoint.CurveType.Straight)
        });
        shape.graphics.SetMeshDirty();
    }


}


