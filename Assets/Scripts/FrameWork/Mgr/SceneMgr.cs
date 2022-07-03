using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景管理类
/// author:cyk
/// </summary>
public class SceneMgr
{
    private static SceneMgr _inst;
    public static SceneMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new SceneMgr();
                _inst._popArr = new List<UIScene>();

            }
            return _inst;
        }
    }
    private List<UIScene> _popArr;
    /**当前场景**/
    public UIScene curScene;
    public string curSceneName;
    /// <summary>
    /// 打开场景（替换模式）
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="data"></param>
    public void Run(string sceneName, object data = null)
    {
        ShowScene(sceneName, data);
    }

    /// <summary>
    /// 打开场景（入栈模式）
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="data"></param>
    public void Push(string sceneName, object data = null)
    {
        ShowScene(sceneName, data, true);
    }

    private void ShowScene(string sceneName, object data = null, bool toPush = false)
    {
        if (curScene != null && curScene.ClassName == sceneName) return;//相同场景
        ModuleCfgInfo moduleInfo = ModuleMgr.inst.GetModuleInfo(sceneName);
        if (moduleInfo == null)
        {
            Debug.LogError("未注册模块：" + sceneName);
            return;
        }
        curSceneName = sceneName;
        if (moduleInfo.preResList != null && moduleInfo.preResList.Length > 0)
        {
            //todo....
            foreach (var item in moduleInfo.preResList)
            {
                UIPackage.AddPackage(item);
            }
            OnUILoaded(moduleInfo, data, toPush);
            //ResMgr.inst.load(moduleInfo.preResList, this.OnUILoaded.bind(this, moduleInfo, data, toPush));
        }
        else
        {
            OnUILoaded(moduleInfo, data, toPush);
        }

    }

    private void OnUILoaded(ModuleCfgInfo moduleInfo, object data, bool toPush)
    {
        if (toPush && curScene != null)
        {
            _popArr.Add(curScene);
            curScene.RemoveSelf();
        }
        else
        {
            checkDestoryLastScene(!toPush);
        }

        curScene = BaseUT.Inst.CreateClassByName<UIScene>(moduleInfo.name);
        curScene.gameObjectName = moduleInfo.name;
        BaseUT.Inst.SetFitSize(curScene);
        GRoot.inst.AddChild(curScene);
        if (data != null) curScene.SetData(data);
    }

    /**判断销毁上个场景并释放资源 */
    private void checkDestoryLastScene(bool destory = false)
    {
        if (curScene != null)
        {
            ModuleCfgInfo lastModuleInfo = ModuleMgr.inst.GetModuleInfo(curScene.gameObjectName);
            if (destory)
            {//销毁上个场景
                curScene.Destory();
                if (!lastModuleInfo.cacheEnabled)
                {
                    //ResMgr.inst.releaseResModule(this.curScene.className);//释放场景资源
                }
            }
        }
    }

    /** 返回到上个场景*/
    public void pop()
    {
        if (_popArr.Count <= 0)
        {
            Debug.LogError("已经pop到底了！！！！！！！");
            return;
        }
        checkDestoryLastScene(true);

        curScene = _popArr[_popArr.Count];
        _popArr.RemoveAt(_popArr.Count - 1);
        curSceneName = curScene.gameObjectName;
        curScene.AddSelfToOldParent();
    }

}
