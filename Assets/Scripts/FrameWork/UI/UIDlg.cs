using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDlg : UILayer
{
    public override GComponent GetParent()
    {
        return SceneMgr.inst.curScene.dlg;
    }
}
