using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SubLayerMgr
{
    private Dictionary<string, Type> _classMap;
    private Dictionary<string, UILayer> _scriptMap;
    public UILayer curLayer;
    private List<UILayer> _popArr;
    public SubLayerMgr()
    {
        _classMap = new Dictionary<string, Type>();
        _scriptMap = new Dictionary<string, UILayer>();
        _popArr = new List<UILayer>();
    }

    /**
    * 注册子页面
    * @param layerClass 
    */
    public void Register(string layerName, object opt = null)
    {
        _classMap[layerName] = Type.GetType(layerName);
    }

    /**显示指定界面（替换模式） */
    public void Run(string layerName, object data = null)
    {
        _show(layerName, data);
    }

    /**显示指定界面（入栈模式） */
    public void Push(string layerName, object data = null)
    {
        _show(layerName, data, true);
    }

    private void _show(string layerName, object data, bool toPush = false)
    {
        if (curLayer != null && curLayer.ClassName == layerName) return;//打开同个界面

        Type registerLayer = _classMap[layerName];
        bool needDestory = registerLayer == null && !toPush;//未注册  && 非入栈模式

        CheckDestoryLastLayer(needDestory);

        if (curLayer != null)
        {
            if (toPush) _popArr.Add(curLayer);
            if (toPush || !needDestory)
            {
                curLayer.RemoveSelf();
            }
        }

        if (_scriptMap.TryGetValue(layerName, out UILayer layer))
        {
            curLayer = layer;
            curLayer.AddSelfToOldParent();
            return;
        }

        curLayer = UILayer.Show(layerName, data);
        if (_classMap[layerName] != null)
        {
            _scriptMap[layerName] = curLayer;
        }
    }

    /**判断销毁上个界面并释放资源 */
    private void CheckDestoryLastLayer(bool destory = false)
    {
        if (destory && curLayer != null && !curLayer.hasDestory)
        {
            curLayer.Close();
        }
    }

    /** layer出栈*/
    public void Pop()
    {
        if (_popArr.Count <= 0)
        {
            Debug.LogError("已经pop到底了！！！！！！！");
            return;
        }
        CheckDestoryLastLayer(true);
        curLayer = _popArr[_popArr.Count];
        _popArr.RemoveAt(_popArr.Count - 1);
        curLayer.AddSelfToOldParent();
    }

    /**清除所有注册的layer */
    public void ReleaseAllLayer()
    {
        CheckDestoryLastLayer(true);
        foreach (var item in _popArr)
        {
            if (!item.hasDestory) item.Close();
        }

        foreach (var item in _scriptMap)
        {
            if (!item.Value.hasDestory)
            {
                item.Value.Close();
            }
        }

        _popArr = new List<UILayer>();
    }

    public void Dispose()
    {
        ReleaseAllLayer();
        _classMap = null;
        _popArr = null;
    }
}
