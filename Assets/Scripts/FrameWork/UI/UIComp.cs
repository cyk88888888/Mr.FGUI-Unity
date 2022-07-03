using FairyGUI;
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
        GComponent compSkin = UIPackage.CreateObject(PkgName, CompName).asCom;
        SetSize(compSkin.width, compSkin.height);
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
        Debug.Log("进入" + className);
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
        Debug.Log("children: " + children.Length);
        for (int i = 0; i < children.Length; i++)
        {

        }
    }

    public void SetData(object _data)
    {
        if (_data == data) return;
        __data = _data;
        Dchg();
    }


    public string className
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

    public void close()
    {
        //onCloseAnimation(() => {
        Destory();
        //});
    }
    /** 添加到旧父级（用于界面回退管理，开发者请勿调用）**/
    public void AddSelfToOldParent()
    {
        __doEnter();
        SetParent(_oldParent);
    }
    /** 从父级移除（用于界面回退管理，开发者请勿调用）**/
    public void RemoveSelf()
    {
        _dispose();
        RemoveFromParent();
    }

    private void _dispose()
    {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("退出" + className);
        OnExit_b();
        OnExit();
        OnExit_a();
    }

    /** 销毁*/
    public void Destory()
    {
        if (hasDestory) return;
        Debug.Log("onDestroy: " + className);
        Dispose();
        hasDestory = true;
    }
}
