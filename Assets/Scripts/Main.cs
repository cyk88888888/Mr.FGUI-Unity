using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ������ļ�
/// author��cyk
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
        view.displayObject.gameObject.AddComponent<LoadingLayer>();
        GRoot.inst.AddChild(view);

        Invoke("onInvoke", 1);
    }

    private void onInvoke()
    {
        //GComponent firstChildren = GRoot.inst.GetChildAt(0).asCom;
        //firstChildren.Dispose();
        GRoot.inst.RemoveChildren();
        Debug.Log("ɾ��GRoot����Children");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
