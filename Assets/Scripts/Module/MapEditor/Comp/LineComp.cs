using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ����
/// </summary>
public class LineComp : GComponent
{
    
    public LineComp()
    {
        touchable = false;
        GComponent line = UIPackage.CreateObject("Common", "LineComp").asCom;
        AddChild(line);
    }

}


