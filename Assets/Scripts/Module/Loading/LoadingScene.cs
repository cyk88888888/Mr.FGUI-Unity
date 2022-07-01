using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 游戏登入加载场景
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
