using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 主入口文件
/// author：cyk
/// </summary>
public class Main : MonoBehaviour
{
    private void Awake()
    {
        UIPackage.AddPackage("UI/Loading");
    }
    // Start is called before the first frame update
    void Start()
    {
        GComponent view = UIPackage.CreateObject("Loading", "Loading").asCom;
        GRoot.inst.AddChild(view);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
