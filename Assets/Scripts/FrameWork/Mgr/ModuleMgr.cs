using FairyGUI;
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
            }
            return _inst;
        }
    }

    public Dictionary<string, ModuleCfgInfo> moduleDic;
    /// <summary>
    /// ��ʼ������ģ��
    /// </summary>
    public void initModule()
    {
        moduleDic["LoadingScene"] = new ModuleCfgInfo("LoadingScene", false, null);
    }

    /// <summary>
    /// ��ȡģ����Ϣ
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    public ModuleCfgInfo getModuleInfo(string moduleName)
    {
        ModuleCfgInfo info;
        if (!moduleDic.TryGetValue(moduleName, out info))
        {
            Debug.LogError("δע��ģ�飺" + moduleName);
        }
        return info;
    }

    /// <summary>
    /// ��ȡ���Ƥ��
    /// </summary>
    /// <param name="pkgName">����</param> 
    /// <param name="compName">�����</param> 
    /// <returns></returns>
    public GComponent getGCompSkin(string pkgName, string compName)
    {
        return UIPackage.CreateObject(pkgName, compName).asCom;
    }
}
