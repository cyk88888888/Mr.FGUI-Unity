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
    * ע����ҳ��
    * @param layerClass 
    */
    public void Register(string layerName, object opt = null)
    {
        _classMap[layerName] = Type.GetType(layerName);
    }

    /**��ʾָ�����棨�滻ģʽ�� */
    public void Run(string layerName, object data = null)
    {
        _show(layerName, data);
    }

    /**��ʾָ�����棨��ջģʽ�� */
    public void Push(string layerName, object data = null)
    {
        _show(layerName, data, true);
    }

    private void _show(string layerName, object data, bool toPush = false)
    {
        if (curLayer != null && curLayer.ClassName == layerName) return;//��ͬ������

        Type registerLayer = _classMap[layerName];
        bool needDestory = registerLayer == null && !toPush;//δע��  && ����ջģʽ

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

    /**�ж������ϸ����沢�ͷ���Դ */
    private void CheckDestoryLastLayer(bool destory = false)
    {
        if (destory && curLayer != null && !curLayer.hasDestory)
        {
            curLayer.Close();
        }
    }

    /** layer��ջ*/
    public void Pop()
    {
        if (_popArr.Count <= 0)
        {
            Debug.LogError("�Ѿ�pop�����ˣ�������������");
            return;
        }
        CheckDestoryLastLayer(true);
        curLayer = _popArr[_popArr.Count];
        _popArr.RemoveAt(_popArr.Count - 1);
        curLayer.AddSelfToOldParent();
    }

    /**�������ע���layer */
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
