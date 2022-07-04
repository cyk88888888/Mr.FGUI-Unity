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

    protected override void Ctor()
    {
        UIPackage.AddPackage("UI/TurnPage");
        UIObjectFactory.SetPackageItemExtension("ui://TurnPage/Book", typeof(FairyBook));
        UIObjectFactory.SetPackageItemExtension("ui://TurnPage/Page", typeof(BookPage));
    }

    private GButton btn_bag;
    private GButton btn_loopList;
    protected override void OnEnter()
    {
        btn_bag = view.GetChild("btn_bag").asButton;
        btn_bag.onClick.Add(() => {
            Show("BagDlg");
        });

        btn_loopList = view.GetChild("btn_loopList").asButton;
        btn_loopList.onClick.Add(() => {
            SceneMgr.inst.Push("LoopListScene");
        });
    }
}


