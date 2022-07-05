using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 游戏登入加载界面
/// </summary>
public class LoadingLayer: UILayer
{
    protected override string PkgName
    {
        get { return "Loading"; }
    }

    protected override string CompName
    {
        get { return "LoadingLayer"; }
    }

    private GProgressBar bar;
    private bool isEntering;
    protected override void OnEnter()
    {
        UIPackage.AddPackage("UI/Common");
        bar = view.GetChild("bar").asProgress;
        Timers.inst.AddUpdate(OnTick);
    }

    private void OnTick(object param)
    {
        bar.value += 1;
        if (!isEntering && bar.value >= 100)
        {
            isEntering = true;
            SceneMgr.inst.Run("HomeScene");
            Timers.inst.Remove(OnTick);
        }
    }
}
