using System;
using System.Collections;
using System.Collections.Generic;
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
    public void register(string layerName, object opt = null)
    {
        _classMap[layerName] = Type.GetType(layerName);
    }

    /**显示指定界面（替换模式） */
    public void run(string layerName, object data = null)
    {
        _show(layerName, data);
    }

    /**显示指定界面（入栈模式） */
    public void push(string layerName, object data = null)
    {
        _show(layerName, data, true);
    }

    private void _show(string layerName, object data, bool toPush = false)
    {
        if (curLayer && curLayer.className == layerName) return;//打开同个界面

        Type registerLayer = _classMap[layerName];
        bool needDestory = registerLayer == null && !toPush;//未注册  && 非入栈模式

        checkDestoryLastLayer(needDestory);

        if (curLayer != null)
        {
            if (toPush) _popArr.Add(curLayer);
            if (toPush || !needDestory)
            {
                curLayer.removeSelf();
            }
        }

        if (_scriptMap.TryGetValue(layerName, out UILayer script))
        {
            curLayer = _scriptMap[layerName];
            curLayer.addSelf();
            return;
        }

        curLayer = (UILayer)BaseUT.Inst.GetUIComp(layerName, data);
        curLayer.setParent(curLayer.getParent());
        if (_classMap[layerName] != null)
        {
            _scriptMap[layerName] = curLayer;
        }
    }

    /**判断销毁上个界面并释放资源 */
    private void checkDestoryLastLayer(bool destory = false)
    {
        if (destory && curLayer && !curLayer.hasDestory)
        {
            curLayer.close();
        }
    }

    /** layer出栈*/
    public void pop()
    {
        if (_popArr.Count <= 0)
        {
            Debug.LogError("已经pop到底了！！！！！！！");
            return;
        }
        checkDestoryLastLayer(true);
        curLayer = _popArr[_popArr.Count];
        _popArr.RemoveAt(_popArr.Count - 1);
        curLayer.addSelf();
    }

    /**清除所有注册的layer */
    public void releaseAllLayer()
    {
        checkDestoryLastLayer(true);
        foreach (var item in _popArr)
        {
            if (!item.hasDestory) item.close();
        }

        foreach (var item in _scriptMap)
        {
            if (!item.Value.hasDestory)
            {
                item.Value.close();
            }
        }

        _popArr = new List<UILayer>();
    }

    public void dispose()
    {
        releaseAllLayer();
        _classMap = null;
        _popArr = null;
    }
}
