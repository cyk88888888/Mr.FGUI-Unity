using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基础工具类
/// author: cyk
/// </summary>
public class BaseUT
{
    private static BaseUT _inst;
    public static BaseUT Inst
    {
        get
        {
            if (_inst == null)
                _inst = new BaseUT();
            return _inst;
        }
    }

    /// <summary>
    /// 获取组件皮肤
    /// </summary>
    /// <param name="pkgName">包名</param> 
    /// <param name="compName">组件名</param> 
    /// <returns></returns>
    public GComponent GetGCompSkin(string pkgName, string compName)
    {
        return UIPackage.CreateObject(pkgName, compName).asCom;
    }

    public UIComp GetUIComp(string layerName, object data = null)
    {
        ModuleMgr.inst.pkgDic.TryGetValue(layerName, out PkgInfo pkgInfo);
        if (pkgInfo == null)
        {
            Debug.LogError("请先注册Layer包路径：" + layerName);
            return null;
        }
        GComponent compSkin = GetGCompSkin(pkgInfo.pkgName, pkgInfo.compName);
        UIComp script = (UIComp)compSkin.displayObject.gameObject.AddComponent(Type.GetType(layerName));
        script.setView(compSkin);
        script.setData(data);
        return script;
    }

    public void SetFitSize(GComponent comp)
    {
        comp.MakeFullScreen();
    }
}
