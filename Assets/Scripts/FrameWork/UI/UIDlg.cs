using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDlg : UILayer
{
    protected override GComponent GetParent()
    {
        return SceneMgr.inst.curScene.dlg;
    }
}
