using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ��Ϸ������س���
/// </summary>
public class LoadingScene : UIScene
{
    protected override void Ctor()
    {
        mainClassLayer = "LoadingLayer";
        List<string> classList = new();
        foreach (var item in classList)
        {
            subLayerMgr.Register(item);
        }
    }
}
