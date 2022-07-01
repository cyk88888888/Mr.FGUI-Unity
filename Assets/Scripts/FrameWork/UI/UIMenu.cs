using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : UILayer
{
    public override GComponent getParent()
    {
        return SceneMgr.inst.curSceneScript.menuLayer;
    }
}
