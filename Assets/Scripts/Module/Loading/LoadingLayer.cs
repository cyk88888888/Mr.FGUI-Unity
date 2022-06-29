using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 游戏登入加载界面
/// </summary>
public class LoadingLayer : MonoBehaviour
{
    private GComponent _mainView;
    void Awake()
    {
        //UIPackage.AddPackage("UI/Loading");
    }
    // Start is called before the first frame update
    void Start()
    {
        _mainView = this.GetComponent<UIPanel>().ui;
        Debug.Log(_mainView.width);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
