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
    /// <summary>
    /// 包名
    /// </summary>
    public static string pkgName = "";
    /// <summary>
    /// 组件名
    /// </summary>
    public static string compName = "";
    /// <summary>
    /// 皮肤
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
        Debug.Log("进入" + gameObject.name);
    }

    private void _dispose() {
        //self.clearAllTimeoutOrInterval();
        //self.rmAllTweens();

        Debug.Log("退出" + gameObject.name);
        onExit_b();
        onExit();
        onExit_a();
    }
}
