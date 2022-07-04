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

        Object prefab = Resources.Load("Role/npc");
        GameObject go = (GameObject)Object.Instantiate(prefab);
        go.transform.localPosition = new Vector3(61, -89, 1000); //set z to far from UICamera is important!
        go.transform.localScale = new Vector3(180, 180, 180);
        go.transform.localEulerAngles = new Vector3(0, 100, 0);
        view.GetChild("holder").asGraph.SetNativeObject(new GoWrapper(go));
    }


}


