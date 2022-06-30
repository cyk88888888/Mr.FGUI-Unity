using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayer : UIComp
{
  
    public static UILayer show()
    {
        GComponent compSkin = ModuleMgr.inst.getGCompSkin(this.name);
    }
}
