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
                _inst = new ModuleMgr
                {
                    moduleDic = new Dictionary<string, ModuleCfgInfo>()
                };
            }
            return _inst;
        }
    }

    public void Init()
    {
        InitModule();
    }

    public Dictionary<string, ModuleCfgInfo> moduleDic;
    /// <summary>
    /// ��ʼ������ģ��
    /// </summary>
    private void InitModule()
    {
        moduleDic["LoadingScene"] = new ModuleCfgInfo("LoadingScene", false, new List<string>() { "UI/Loading" });
        moduleDic["HomeScene"] = new ModuleCfgInfo("HomeScene", true, new List<string>() { "UI/Home", "UI/TurnPage" });
        moduleDic["RoleScene"] = new ModuleCfgInfo("RoleScene", false, new List<string>() { "UI/Role" });
        moduleDic["LoopListScene"] = new ModuleCfgInfo("LoopListScene", false, new List<string>() { "UI/LoopList" });
    }

    /// <summary>
    /// ��ȡģ����Ϣ
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    public ModuleCfgInfo GetModuleInfo(string moduleName)
    {
        moduleDic.TryGetValue(moduleName, out ModuleCfgInfo info);
        return info;
    }

}
