using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMsg : UILayer
{
    protected override GComponent GetParent()
    {
        return SceneMgr.inst.curScene.msg;
    }
}
