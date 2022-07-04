using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ui场景基类
/// author：cyk
/// </summary>
public class UIScene : GComponent, IEmmiter
{
    protected SubLayerMgr subLayerMgr;
    public GComponent layer;
    public GComponent dlg;
    public GComponent msg;
    public GComponent menuLayer;
    protected string mainClassLayer;
    private bool _isFirstEnter = true;
    protected object _moduleParam;
    private Dictionary<string, EventListenerDelegate> notifications = new Dictionary<string, EventListenerDelegate>();
    public UIScene()
    {
        subLayerMgr = new SubLayerMgr();
        Ctor_b();
        Ctor();
        Ctor_a();
        onAddedToStage.Add(Init);
    }

    protected virtual void Ctor_b() { }
    protected virtual void Ctor() { }
    protected virtual void Ctor_a() { }

    protected virtual void OnEnter_b() { }
    protected virtual void OnEnter() { }
    protected virtual void OnFirstEnter() { }
    protected virtual void OnEnter_a() { }

    protected virtual void OnExit_b() { }
    protected virtual void OnExit() { }
    protected virtual void OnExit_a() { }

    private void Init()
    {
        onAddedToStage.Remove(Init);
        InitLayer();
        if (mainClassLayer != null)
        {
            subLayerMgr.Register(mainClassLayer);
            Push(mainClassLayer);
        }
    }

    private void InitLayer()
    {
        layer = AddGCom2GRoot("UILayer");
        menuLayer = AddGCom2GRoot("UIMenuLayer");
        dlg = AddGCom2GRoot("UIDlg");
        msg = AddGCom2GRoot("UIMsg");
        __doEnter();
    }

    /**
    * 添加层级容器到GRoot
    * @param name 名称
    * @returns 
    */
    private GComponent AddGCom2GRoot(string name)
    {
        GComponent newNode = new();
        newNode.gameObjectName = name;
        SceneMgr.inst.curScene.AddChild(newNode);
        BaseUT.Inst.SetFitSize(newNode);
        return newNode;
    }

    private void __doEnter()
    {
        Debug.Log("进入" + ClassName);
        OnEnter_b();
        OnEnter();
        if (_isFirstEnter)
        {
            _isFirstEnter = false;
            OnFirstEnter();
        }
        OnEnter_a();
    }

    public void SetData(object data)
    {
        _moduleParam = data;
    }

    public void Emit(string notificationName)
    {
        Emmiter.Emit(new EventCallBack(notificationName));
    }
    public void Emit(string notificationName, object[] body)
    {
        Emmiter.Emit(new EventCallBack(notificationName, body));
    }
    public void Emit(string notificationName, object[] body, object sender)
    {
        Emmiter.Emit(new EventCallBack(notificationName, body, sender));
    }
    public void OnEmitter(string type, EventListenerDelegate listener)
    {
        if (!notifications.ContainsKey(type))
        {
            Emmiter.On(type, listener);
            notifications.Add(type, listener);
        }
    }
    public void UnEmitter(string type, EventListenerDelegate listener)
    {
        if (notifications.ContainsKey(type))
        {
            Emmiter.Off(type, listener);
        }
    }

    public string ClassName
    {
        get
        {
            return gameObjectName;
        }
    }
    /**重置到主界面（会清掉当前堆栈中的所有界面） */
    public void ResetToMain()
    {
        ReleaseAllLayer();
        Push(mainClassLayer, null);
    }

    /**显示指定界面（替换模式） */
    public void Run(string layerName, object data = null)
    {
        subLayerMgr.Run(layerName, data);
    }

    /**显示指定界面（入栈模式） */
    public void Push(string layerName, object data = null)
    {
        subLayerMgr.Push(layerName, data);
    }

    /**layer出栈 */
    public void Pop()
    {
        subLayerMgr.Pop();
    }

    /// <summary>
    /// 将场景添加到GRoot根节点（用于界面回退管理，开发者请勿调用）**/
    /// </summary>
    public void AddSelfToOldParent()
    {
        DisposeByParent(layer,true);
        DisposeByParent(dlg, true);
        DisposeByParent(msg, true);
        DisposeByParent(menuLayer, true);
        __doEnter();
        GRoot.inst.AddChild(SceneMgr.inst.curScene);
    }
    /// <summary>
    /// 从父级移除（用于界面回退管理，开发者请勿调用）**/
    /// </summary>
    public void RemoveSelf()
    {
        DisposeByParent(layer);
        DisposeByParent(dlg);
        DisposeByParent(msg);
        DisposeByParent(menuLayer);
        _dispose();
        SceneMgr.inst.curScene.RemoveFromParent();
    }

    /**清除所有layer */
    private void ReleaseAllLayer()
    {
        subLayerMgr.ReleaseAllLayer();
    }

    private void _dispose()
    {
        foreach (var item in notifications)
        {
            Emmiter.Off(item.Key, item.Value);
        }
        notifications.Clear();

        Debug.Log("退出" + ClassName);
        OnExit_b();
        OnExit();
        OnExit_a();
    }

    private void DisposeByParent(GComponent _parent,bool isEnter = false)
    {
        foreach (var item in _parent.GetChildren())
        {
            if (isEnter)
            {
                (item as UIComp).__doEnter();
            }
            else
            {
                (item as UIComp).__dispose();
            }
        }
    }

    private void Destory()
    {
        subLayerMgr.Dispose();
        subLayerMgr = null;
        Debug.Log("onDestroy: " + ClassName);
        Dispose();
    }

    public void Close()
    {
        _dispose();
        Destory();
    }

}
