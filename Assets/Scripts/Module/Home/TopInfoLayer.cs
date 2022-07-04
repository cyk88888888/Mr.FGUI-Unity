using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 主界面底部按钮
/// </summary>
public class TopInfoLayer : UIMenu
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "TopInfoLayer"; }
    }

    private GComponent tap_head;
    protected override void OnEnter()
    {
        tap_head = view.GetChild("tap_head").asCom;
        tap_head.onClick.Add(() => {
            SceneMgr.inst.Push("RoleScene");
        });
    }


   


}


