using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// author��cyk
/// </summary>
public class UIComp : GComponent
{
    /// Ƥ��
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
    /// ����
    /// </summary>
    protected virtual string PkgName
    {
        get { return ""; }
    }
    /// <summary>
    /// �����
    /// </summary>
    protected virtual string CompName
    {
        get { return ""; }
    }

    /// <summary>
    /// ��ʼ��UI
    /// </summary>
    private void Init()
    {
        onAddedToStage.Remove(Init);
        _oldParent = parent;
        if (PkgName == "" || CompName == "")
        {
            Debug.LogError("�����ڶ�Ӧ������дPkgName��CompName�ֶΣ�����");
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
        Debug.Log("����" + ClassName);
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
    /// ����view�ĸ���
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(GComponent parent)
    {
        if (view == null) return;
        _oldParent = parent;
        parent.AddChild(view);
    }

    /// <summary>
    /// �������
    /// </summary>
    public void Close()
    {
        //onCloseAnimation(() => {
        _dispose();
        Destory();
        //});
    }
    /// <summary>
    /// ��ӵ��ɸ��������ڽ�����˹���������������ã�
    /// </summary>
    public void AddSelfToOldParent()
    {
        __doEnter();
        SetParent(_oldParent);
    }
    /// <summary>
    /// �Ӹ����Ƴ������ڽ�����˹���������������ã�
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

        Debug.Log("�˳�" + ClassName);
        foreach (var item in childComp)
        {
            item._dispose();
        }
        OnExit_b();
        OnExit();
        OnExit_a();
    }

    /** ����*/
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
