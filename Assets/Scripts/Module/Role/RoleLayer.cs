using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 人物信息界面
/// </summary>
public class RoleLayer : UILayer
{
    protected override string PkgName
    {
        get { return "Role"; }
    }

    protected override string CompName
    {
        get { return "RoleLayer"; }
    }
    private GButton btn_back;
    protected override void OnEnter()
    {
        btn_back = view.GetChild("btn_back").asButton;
        btn_back.onClick.Add(() =>
        {
            SceneMgr.inst.pop();
        });

        Object prefab = Resources.Load("Role/npc");
        GameObject go = (GameObject)Object.Instantiate(prefab);
        go.transform.localPosition = new Vector3(61, -89, 1000); //set z to far from UICamera is important!
        go.transform.localScale = new Vector3(180, 180, 180);
        go.transform.localEulerAngles = new Vector3(0, 100, 0);
        view.GetChild("holder").asGraph.SetNativeObject(new GoWrapper(go));
    }

}

