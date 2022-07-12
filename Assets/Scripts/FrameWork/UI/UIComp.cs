using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 组件基类
/// author：cyk
/// </summary>
public class UIComp : GComponent, IEmmiter
{
    /// 皮肤
    /// </summary>
    protected GComponent view;
    private GComponent _oldParent;
    public object __data = null;
    private bool isFirstEnter = true;
    public bool hasDestory = false;
    protected bool sameSizeWithView = true;//脚本容器宽高是否需要和view一样
    private bool needCreateView = true;
    private Dictionary<string, UIComp> childCompDic;
    private Dictionary<string, EventListenerDelegate> notifications = new Dictionary<string, EventListenerDelegate>();
    public UIComp()
    {
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

    protected virtual void Dchg() { }

    protected virtual void OnExit_b() { }
    protected virtual void OnExit() { }
    protected virtual void OnExit_a() { }

    /// <summary>
    /// 包名
    /// </summary>
    protected virtual string PkgName
    {
        get { return ""; }
    }
    /// <summary>
    /// 组件名
    /// </summary>
    protected virtual string CompName
    {
        get { return ""; }
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    private void Init()
    {
        onAddedToStage.Remove(Init);
        if (!needCreateView) return;
        _oldParent = parent;
        if (PkgName == "" || CompName == "")
        {
            Debug.LogError("请先在对应界面重写PkgName和CompName字段！！！");
            return;
        }
        GComponent compSkin = UIPackage.CreateObject(PkgName, CompName).asCom;
        AddChild(compSkin);
        SetView(compSkin);
    }
    public void SetView(GComponent _view)
    {
        view = _view;
        if (sameSizeWithView) SetSize(view.viewWidth, view.viewHeight);
        __doEnter();
    }
    public void __doEnter()
    {
        Debug.Log("进入" + ClassName);
        OnEnter_b();
        OnEnter();
        if (isFirstEnter)
        {
            isFirstEnter = false;
            OnFirstEnter();
        }
        OnEnter_a();
        InitProperty();
    }

    protected void InitProperty()
    {
        GObject[] children = view.GetChildren();
        if (childCompDic == null) childCompDic = new();
        for (int i = 0; i < children.Length; i++)
        {
            GObject item = children[i];
            if (item is GComponent)
            {
                string gameObjectName = item.gameObjectName;
                Type type = Type.GetType(gameObjectName);
                if (type != null)
                {
                    UIComp childComp_Script;
                    if (!childCompDic.TryGetValue(item.name, out childComp_Script))
                    {
                        childComp_Script = BaseUT.Inst.CreateClassByName<UIComp>(gameObjectName);
                        childComp_Script.gameObjectName = gameObjectName + "_script";
                        childComp_Script.needCreateView = false;
                        childComp_Script.SetParent(SceneMgr.inst.curScene.scripLayer);
                        childCompDic.Add(item.name, childComp_Script);
                    }
                    childComp_Script.SetView((GComponent)item);
                   
                }
            }
        }
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

    public void SetData(object _data)
    {
        if (_data == data) return;
        __data = _data;
        Dchg();
    }


    public string ClassName
    {
        get
        {
            return gameObjectName.Split("_")[0];
        }
    }

    /// <summary>
    /// 设置view的父级
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(GComponent parent)
    {
        _oldParent = parent;
        parent.AddChild(this);
    }

    /// <summary>
    /// 添加到旧父级（用于界面回退管理，开发者请勿调用）
    /// </summary>
    public void AddSelfToOldParent()
    {
        __doEnter();
        SetParent(_oldParent);
    }
    /// <summary>
    /// 从父级移除（用于界面回退管理，开发者请勿调用）
    /// </summary>
    public void RemoveSelf()
    {
        __dispose();
        RemoveFromParent();
    }

    public void __dispose()
    {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("退出" + ClassName);
        if (childCompDic != null)
        {
            foreach (var item in childCompDic)
            {
                item.Value.__dispose();
            }

        }

        foreach (var item in notifications)
        {
            Emmiter.Off(item.Key, item.Value);
        }
        notifications.Clear();

        OnExit_b();
        OnExit();
        OnExit_a();
    }

    /** 销毁*/
    protected void Destory()
    {
        if (hasDestory) return;
        hasDestory = true;

        foreach (var item in childCompDic)
        {
            item.Value.Destory();
        }

        Debug.Log("onDestroy: " + ClassName);
        view.Dispose();
        Dispose();

    }
}
