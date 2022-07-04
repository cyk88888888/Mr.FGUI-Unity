using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ������Ϣ����
/// </summary>
public class RoleScene : UIScene
{
    protected override void Ctor()
    {
        mainClassLayer = "RoleLayer";
        List<string> classList = new();
        foreach (var item in classList)
        {
            subLayerMgr.Register(item);
        }
    }
}
