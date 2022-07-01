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
    * ע����ҳ��
    * @param layerClass 
    */
    public void register(string layerName, object opt = null)
    {
        _classMap[layerName] = Type.GetType(layerName);
    }

    /**��ʾָ�����棨�滻ģʽ�� */
    public void run(string layerName, object data = null)
    {
        _show(layerName, data);
    }

    /**��ʾָ�����棨��ջģʽ�� */
    public void push(string layerName, object data = null)
    {
        _show(layerName, data, true);
    }

    private void _show(string layerName, object data, bool toPush = false)
    {
        if (curLayer && curLayer.className == layerName) return;//��ͬ������

        Type registerLayer = _classMap[layerName];
        bool needDestory = registerLayer == null && !toPush;//δע��  && ����ջģʽ

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

    /**�ж������ϸ����沢�ͷ���Դ */
    private void checkDestoryLastLayer(bool destory = false)
    {
        if (destory && curLayer && !curLayer.hasDestory)
        {
            curLayer.close();
        }
    }

    /** layer��ջ*/
    public void pop()
    {
        if (_popArr.Count <= 0)
        {
            Debug.LogError("�Ѿ�pop�����ˣ�������������");
            return;
        }
        checkDestoryLastLayer(true);
        curLayer = _popArr[_popArr.Count];
        _popArr.RemoveAt(_popArr.Count - 1);
        curLayer.addSelf();
    }

    /**�������ע���layer */
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
