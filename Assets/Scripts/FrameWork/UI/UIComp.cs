using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// author��cyk
/// </summary>
public class UIComp : GComponent, IEmmiter
{
    /// Ƥ��
    /// </summary>
    protected GComponent view;
    private GComponent _oldParent;
    public object __data = null;
    private bool isFirstEnter = true;
    public bool hasDestory = false;
    protected bool sameSizeWithView = true;//�ű���������Ƿ���Ҫ��viewһ��
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
        if (!needCreateView) return;
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
        if (sameSizeWithView) SetSize(view.viewWidth, view.viewHeight);
        __doEnter();
    }
    public void __doEnter()
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
    /// ����view�ĸ���
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(GComponent parent)
    {
        _oldParent = parent;
        parent.AddChild(this);
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
        __dispose();
        RemoveFromParent();
    }

    public void __dispose()
    {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("�˳�" + ClassName);
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

    /** ����*/
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
