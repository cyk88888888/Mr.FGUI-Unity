using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ui场景基类
/// author：cyk
/// </summary>
public class UIScene : MonoBehaviour
{
    protected SubLayerMgr subLayerMgr;
    public GComponent layer;
    public GComponent dlg;
    public GComponent msg;
    public GComponent menuLayer;
    protected string mainClassLayer;
    private bool _isFirstEnter = true;
    protected object _moduleParam;
    public UIScene()
    {
        subLayerMgr = new SubLayerMgr();
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

    protected virtual void onExit_b() { }
    protected virtual void onExit() { }
    protected virtual void onExit_a() { }

    private void Start()
    {
        initLayer();

        if (mainClassLayer != null)
        {
            subLayerMgr.register(mainClassLayer);
            push(mainClassLayer);
        }
    }

    private void initLayer()
    {
        layer = addGCom2GRoot("UILayer");
        menuLayer = addGCom2GRoot("UIMenuLayer");
        dlg = addGCom2GRoot("UIDlg");
        msg = addGCom2GRoot("UIMsg");
    }

    /**
    * 添加层级容器到GRoot
    * @param name 名称
    * @returns 
    */
    private GComponent addGCom2GRoot(string name)
    {
        GComponent newNode = new GComponent();
        newNode.gameObjectName = name;
        SceneMgr.inst.curScene.AddChild(newNode);
        BaseUT.Inst.SetFitSize(newNode);
        return newNode;
    }

    private void __doEnter()
    {
        Debug.Log("进入" + gameObject.name);
        onEnter_b();
        onEnter();
        if (_isFirstEnter)
        {
            _isFirstEnter = false;
            onFirstEnter();
        }
        onEnter_a();
    }

    public void setData(object data)
    {
        _moduleParam = data;
    }

    void OnEnable()
    {
        __doEnter();
    }

    void OnDisable()
    {
        _dispose();
    }

    public string className
    {
        get
        {
            return gameObject.name;
        }
    }
    /**重置到主界面（会清掉当前堆栈中的所有界面） */
    public void resetToMain()
    {
        releaseAllLayer();
        push(mainClassLayer, null);
    }

    /**显示指定界面（替换模式） */
    public void run(string layerName, object data = null)
    {
        subLayerMgr.run(layerName, data);
    }

    /**显示指定界面（入栈模式） */
    public void push(string layerName, object data = null)
    {
        subLayerMgr.push(layerName, data);
    }

    /**layer出栈 */
    public void pop()
    {
        subLayerMgr.pop();
    }

    /// <summary>
    /// 将场景添加到GRoot根节点
    /// </summary>
    public void addToGRoot()
    {
        GRoot.inst.AddChild(SceneMgr.inst.curScene);
    }

    public void removeFromParent()
    {
        SceneMgr.inst.curScene.RemoveFromParent();
    }

    /**清除所有layer */
    public void releaseAllLayer()
    {
        this.subLayerMgr.releaseAllLayer();
    }

    public void disposeSubLayerMgr()
    {
        this.subLayerMgr.dispose();
        this.subLayerMgr = null;
    }

    public void _dispose()
    {
        //if (_emmitMap != null)
        //{
            //        for (let event in _emmitMap) {
            //    unEmitter(event, _emmitMap [event]);
            //}
            //_emmitMap = null;
        //}
        Debug.Log("退出" + gameObject.name);
        onExit_b();
        onExit();
        onExit_a();
    }

    public void destory()
    {
        _dispose();
        subLayerMgr.dispose();
        subLayerMgr = null;
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        Debug.Log("onDestroy: " + gameObject.name);
    }
}
