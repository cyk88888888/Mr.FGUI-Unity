using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ��Ϸ������
/// </summary>
public class HomeScene : UIScene
{
    protected override void Ctor()
    {
        mainClassLayer = "HomeLayer";
        List<string> classList = new();
        foreach (var item in classList)
        {
            subLayerMgr.Register(item);
        }
    }
}
