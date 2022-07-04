using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : UILayer
{
    protected override GComponent GetParent()
    {
        return SceneMgr.inst.curScene.menuLayer;
    }
}
