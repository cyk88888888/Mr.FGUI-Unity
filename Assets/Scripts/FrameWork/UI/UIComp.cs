using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// author��cyk
/// </summary>
public class UIComp : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    public static string pkgName = "";
    /// <summary>
    /// �����
    /// </summary>
    public static string compName = "";
    /// <summary>
    /// Ƥ��
    /// </summary>
    protected GComponent view;
    public object data;
    private bool isFirstEnter = true;

    public UIComp()
    {
        ctor_b();
        ctor();
        ctor_a();
    }

    protected void ctor_b() { }
    protected void ctor() { }
    protected void ctor_a() { }

    protected void onEnter_b() { }
    protected void onEnter() { }
    protected void onFirstEnter() { }
    protected void onEnter_a() { }

    protected void onExit_b() { }
    protected void onExit() { }
    protected void onExit_a() { }

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

    private void __doEnter()
    {
        Debug.Log("����" + gameObject.name);
    }

    private void _dispose() {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("�˳�" + gameObject.name);
        onExit_b();
        onExit();
        onExit_a();
    }
}
