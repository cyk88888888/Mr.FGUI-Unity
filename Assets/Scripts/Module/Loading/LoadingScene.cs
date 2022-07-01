using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ��Ϸ������س���
/// </summary>
public class LoadingScene : UIScene
{
    protected override void ctor()
    {
        mainClassLayer = "LoadingLayer";
        List<string> classList = new();
        foreach (var item in classList)
        {
            subLayerMgr.register(item);
        }
    }
}
