using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ѭ���б���
/// </summary>
public class LoopListScene : UIScene
{
    protected override void Ctor()
    {
        mainClassLayer = "LoopListLayer";
        List<string> classList = new();
        foreach (var item in classList)
        {
            subLayerMgr.Register(item);
        }
    }
}
