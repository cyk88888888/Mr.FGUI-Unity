using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 组件基类
/// author：cyk
/// </summary>
public class UIComp : GComponent
{
    /// 皮肤
    /// </summary>
    protected GComponent view;
    private GComponent _oldParent;
    public object __data = null;
    private bool isFirstEnter = true;
    public bool hasDestory = false;
    private List<UIComp> childComp;
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
        __doEnter();
    }
    private void __doEnter()
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
        if (childComp == null) childComp = new();
        for (int i = 0; i < children.Length; i++)
        {
            GObject item = children[i];
            if (item is GComponent)
            {
                string gameObjectName = item.gameObjectName;
                Type type = Type.GetType(gameObjectName);
                if (type != null)
                {
                    UIComp comp = BaseUT.Inst.CreateClassByName<UIComp>(gameObjectName);
                    comp.gameObjectName = gameObjectName;
                    comp.SetView((GComponent)item);
                    childComp.Add(comp);
                }
            }
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
        if (view == null) return;
        _oldParent = parent;
        parent.AddChild(view);
    }

    /// <summary>
    /// 销毁组件
    /// </summary>
    public void Close()
    {
        //onCloseAnimation(() => {
        _dispose();
        Destory();
        //});
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
        _dispose();
        RemoveFromParent();
    }

    private void _dispose()
    {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("退出" + ClassName);
        foreach (var item in childComp)
        {
            item._dispose();
        }
        OnExit_b();
        OnExit();
        OnExit_a();
    }

    /** 销毁*/
    private void Destory()
    {
        if (hasDestory) return;
        hasDestory = true;
        foreach (var item in childComp)
        {
            item.Destory();
        }
        Debug.Log("onDestroy: " + ClassName);
        Dispose();
        
    }
}
