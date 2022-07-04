using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ui��������
/// author��cyk
/// </summary>
public class UIScene : GComponent
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
    * ��Ӳ㼶������GRoot
    * @param name ����
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
        Debug.Log("����" + ClassName);
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

    public string ClassName
    {
        get
        {
            return gameObjectName;
        }
    }
    /**���õ������棨�������ǰ��ջ�е����н��棩 */
    public void ResetToMain()
    {
        ReleaseAllLayer();
        Push(mainClassLayer, null);
    }

    /**��ʾָ�����棨�滻ģʽ�� */
    public void Run(string layerName, object data = null)
    {
        subLayerMgr.Run(layerName, data);
    }

    /**��ʾָ�����棨��ջģʽ�� */
    public void Push(string layerName, object data = null)
    {
        subLayerMgr.Push(layerName, data);
    }

    /**layer��ջ */
    public void Pop()
    {
        subLayerMgr.Pop();
    }

    /// <summary>
    /// ��������ӵ�GRoot���ڵ㣨���ڽ�����˹���������������ã�**/
    /// </summary>
    public void AddSelfToOldParent()
    {
        __doEnter();
        GRoot.inst.AddChild(SceneMgr.inst.curScene);
    }
    /// <summary>
    /// �Ӹ����Ƴ������ڽ�����˹���������������ã�**/
    /// </summary>
    public void RemoveSelf()
    {
        _dispose();
        SceneMgr.inst.curScene.RemoveFromParent();
    }

    /**�������layer */
    public void ReleaseAllLayer()
    {
        subLayerMgr.ReleaseAllLayer();
    }

    public void DisposeSubLayerMgr()
    {
        subLayerMgr.Dispose();
        subLayerMgr = null;
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
        Debug.Log("�˳�" + ClassName);
        OnExit_b();
        OnExit();
        OnExit_a();
    }

    public void Destory()
    {
        _dispose();
        subLayerMgr.Dispose();
        subLayerMgr = null;
        Debug.Log("onDestroy: " + ClassName);
        Dispose();
    }

   
}
