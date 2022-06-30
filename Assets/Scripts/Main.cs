using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;
/// <summary>
/// 主入口文件
/// author：cyk
/// </summary>
public class Main : MonoBehaviour
{
    private void Awake()
    {
        ModuleMgr.inst.initModule();
        UIPackage.AddPackage("UI/Loading");
    }
    private GComponent view;
    // Start is called before the first frame update
    void Start()
    {
        view = UIPackage.CreateObject("Loading", "Loading").asCom;
        LoadingLayer aa =  (LoadingLayer)view.displayObject.gameObject.AddComponent(Type.GetType("LoadingLayer"));
        GRoot.inst.AddChild(view);
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
