using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 组件基类
/// author：cyk
/// </summary>
public class UIComp : MonoBehaviour
{
    /// 皮肤
    /// </summary>
    protected GComponent view;
    private GComponent _oldParent;
    public object data = null;
    private bool isFirstEnter = true;
    public bool hasDestory = false;
    public UIComp()
    {
        ctor_b();
        ctor();
        ctor_a();
    }

    protected virtual void ctor_b() { }
    protected virtual void ctor() { }
    protected virtual void ctor_a() { }

    protected virtual void onEnter_b() { }
    protected virtual void onEnter() { }
    protected virtual void onFirstEnter() { }
    protected virtual void onEnter_a() { }

    protected virtual void dchg() { }
    protected virtual void onExit_b() { }
    protected virtual void onExit() { }
    protected virtual void onExit_a() { }
   
    private void Awake()
    {
        Debug.Log("Awake: " + gameObject.name);
    }

    private void OnEnable()
    {
        __doEnter();
    }

    private void OnDisable()
    {
        _dispose();
    }

    private void OnDestroy()
    {
        Debug.Log("onDestroy: " + gameObject.name);
    }
    public void setView(GComponent _view)
    {
        view = _view;
    }

    public void setData(object _data)
    {
        if (_data == data) return;
        data = _data;
        dchg();
    }
    /// <summary>
    /// 设置view的父级
    /// </summary>
    /// <param name="parent"></param>
    public void setParent(GComponent parent)
    {
        if (view == null) return;
        _oldParent = parent;
        //view.onAddedToStage.Add()
        parent.AddChild(view);
    }

    private void __doEnter()
    {
        Debug.Log("进入" + gameObject.name);
        initView();
    }

    private void initView()
    {
        onEnter_b();
        onEnter();
        if (isFirstEnter)
        {
            isFirstEnter = false;
            onFirstEnter();
        }
        onEnter_a();
    }

    public string className
    {
        get {
            return name;
        }
    }
    public void close()
    {
        //onCloseAnimation(() => {
            destory();
        //});
    }

    public void addSelf()
    {
        setParent(_oldParent);
    }

    public void removeSelf()
    {
        view.RemoveFromParent();
    }

    private void _dispose()
    {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("退出" + gameObject.name);
        onExit_b();
        onExit();
        onExit_a();
    }

    public void destory()
    {
        if (hasDestory) return;
        view.Dispose();
        hasDestory = true;
    }
}
