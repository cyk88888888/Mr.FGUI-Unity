using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 循环列表场景
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
