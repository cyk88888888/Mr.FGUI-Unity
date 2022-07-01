using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UILayer : UIComp
{
    /// <summary>
    /// ��ʾUI
    /// </summary>
    /// <param name="pkgName"> ���� </param>
    /// <param name="compName"> �����(��ʵ���ǽű�����)</param>
    /// <param name="data">����</param>
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
    /// ��ӵ��㼶
    /// </summary>
    protected void AddToLayer()
    {
        setParent(getParent());
    }
}
