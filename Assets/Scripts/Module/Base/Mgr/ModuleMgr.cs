using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ģ�������
/// </summary>
public class ModuleMgr
{
    private static ModuleMgr _inst;
    public static ModuleMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new ModuleMgr();
                _inst.moduleDic = new Dictionary<string, ModuleCfgInfo>();
                _inst.pkgDic = new Dictionary<string, PkgInfo>();
            }
            return _inst;
        }
    }

    public void Init()
    {
        InitModule();
        InitPkg();
    }

    public Dictionary<string, ModuleCfgInfo> moduleDic;
    public Dictionary<string, PkgInfo> pkgDic;
    /// <summary>
    /// ��ʼ������ģ��
    /// </summary>
    private void InitModule()
    {
        moduleDic["LoadingScene"] = new ModuleCfgInfo("LoadingScene", false, new string[1] { "UI/Loading" });
    }


    private void InitPkg()
    {
        pkgDic["LoadingLayer"] = new PkgInfo("Loading", "LoadingLayer");
    }

    /// <summary>
    /// ��ȡģ����Ϣ
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    public ModuleCfgInfo GetModuleInfo(string moduleName)
    {
        ModuleCfgInfo info;
        if (!moduleDic.TryGetValue(moduleName, out info))
        {
            Debug.LogError("δע��ģ�飺" + moduleName);
        }
        return info;
    }

}
