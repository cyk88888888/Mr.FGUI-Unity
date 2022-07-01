using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UILayer : UIComp
{
    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="pkgName"> 包名 </param>
    /// <param name="compName"> 组件名(其实就是脚本名称)</param>
    /// <param name="data">数据</param>
    /// <returns></returns>
    public static UILayer Show(string pkgName, string compName, object data = null)
    {
        UILayer script = (UILayer)BaseUT.Inst.GetUIComp(pkgName, compName);
        script.AddToLayer();
        return script;
    }

    public virtual GComponent getParent()
    {
        return SceneMgr.inst.curSceneScript.layer;
    }
    /// <summary>
    /// 添加到层级
    /// </summary>
    protected void AddToLayer()
    {
        setParent(getParent());
    }
}
