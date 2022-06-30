using FairyGUI;
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
                _inst = new ModuleMgr();
                _inst.moduleDic = new Dictionary<string, ModuleCfgInfo>();
            }
            return _inst;
        }
    }

    public Dictionary<string, ModuleCfgInfo> moduleDic;
    /// <summary>
    /// 初始化所有模块
    /// </summary>
    public void initModule()
    {
        moduleDic["LoadingScene"] = new ModuleCfgInfo("LoadingScene", false, null);
    }

    /// <summary>
    /// 获取模块信息
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    public ModuleCfgInfo getModuleInfo(string moduleName)
    {
        ModuleCfgInfo info;
        if (!moduleDic.TryGetValue(moduleName, out info))
        {
            Debug.LogError("未注册模块：" + moduleName);
        }
        return info;
    }

    /// <summary>
    /// 获取组件皮肤
    /// </summary>
    /// <param name="pkgName">包名</param> 
    /// <param name="compName">组件名</param> 
    /// <returns></returns>
    public GComponent getGCompSkin(string pkgName, string compName)
    {
        return UIPackage.CreateObject(pkgName, compName).asCom;
    }
}
