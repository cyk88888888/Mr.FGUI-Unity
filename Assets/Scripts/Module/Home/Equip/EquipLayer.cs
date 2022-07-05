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
        line2.lineWidth = 2;
        line2.roundEdge = true;
        line2.path.Create(new GPathPoint[] {
            new GPathPoint(new Vector3(50, 0, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(50, 200, 0), GPathPoint.CurveType.Straight),
        });
        //shape.graphics.SetMeshDirty();
        //line2.path.Clear();
        line2.path.Create(new GPathPoint[] {
            new GPathPoint(new Vector3(100, 0, 0), GPathPoint.CurveType.Straight),
            new GPathPoint(new Vector3(100, 200, 0), GPathPoint.CurveType.Straight),
        });
        //shape.graphics.SetMeshDirty();
    }


}


