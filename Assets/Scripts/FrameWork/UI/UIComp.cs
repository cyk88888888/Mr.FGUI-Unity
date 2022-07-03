using FairyGUI;
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
        Debug.Log("����" + className);
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
    /// ����view�ĸ���
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
    /** ��ӵ��ɸ��������ڽ�����˹���������������ã�**/
    public void AddSelfToOldParent()
    {
        __doEnter();
        SetParent(_oldParent);
    }
    /** �Ӹ����Ƴ������ڽ�����˹���������������ã�**/
    public void RemoveSelf()
    {
        _dispose();
        RemoveFromParent();
    }

    private void _dispose()
    {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("�˳�" + className);
        OnExit_b();
        OnExit();
        OnExit_a();
    }

    /** ����*/
    public void Destory()
    {
        if (hasDestory) return;
        Debug.Log("onDestroy: " + className);
        Dispose();
        hasDestory = true;
    }
}
