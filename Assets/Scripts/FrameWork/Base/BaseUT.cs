using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����������
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
    /// ��ȡ���Ƥ��
    /// </summary>
    /// <param name="pkgName">����</param> 
    /// <param name="compName">�����</param> 
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
            Debug.LogError("����ע��Layer��·����" + layerName);
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
