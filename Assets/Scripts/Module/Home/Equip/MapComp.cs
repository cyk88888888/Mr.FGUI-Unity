using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// µÿÕºΩÁ√Ê
/// </summary>
public class MapComp : UIComp
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "MapComp"; }
    }

    protected override void OnEnter()
    {
        GComponent lineContainer = view.GetChild("lineContainer").asCom;
        GGraph aa = new GGraph();
        aa.SetSize(50, 50);
        aa.DrawRect(300, 300, 2, Color.black, Color.black);
        aa.SetXY(600, 0);
        lineContainer.SetSize(aa.x + aa.width, aa.y + aa.height);
        lineContainer.AddChild(aa);
    }


}


