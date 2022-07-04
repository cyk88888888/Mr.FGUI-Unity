using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// Ö÷½çÃæ
/// </summary>
public class HomeLayer : UILayer
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "HomeLayer"; }
    }

    private GButton btn_bag;
    protected override void OnEnter()
    {
        btn_bag = view.GetChild("btn_bag").asButton;
        btn_bag.onClick.Add(() => {
            Show("BagDlg");
        });
    }


}


