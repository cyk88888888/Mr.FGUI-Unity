using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 模块管理器
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
    /// 初始化所有模块
    /// </summary>
    private void InitModule()
    {
        moduleDic["LoadingScene"] = new ModuleCfgInfo("LoadingScene", false, new string[1] { "UI/Loading" });
        moduleDic["HomeScene"] = new ModuleCfgInfo("HomeScene", false, new string[1] { "UI/Home" });
    }

    /// <summary>
    /// 获取模块信息
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    public ModuleCfgInfo GetModuleInfo(string moduleName)
    {
        if (!moduleDic.TryGetValue(moduleName, out ModuleCfgInfo info))
        {
            Debug.LogError("未注册模块：" + moduleName);
        }
        return info;
    }

}
