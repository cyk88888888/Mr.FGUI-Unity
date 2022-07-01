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
        ModuleMgr.inst.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneMgr.inst.Run("LoadingScene");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
