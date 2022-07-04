using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 人物信息场景
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
