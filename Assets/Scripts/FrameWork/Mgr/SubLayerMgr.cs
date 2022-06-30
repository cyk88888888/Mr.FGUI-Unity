using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLayerMgr
{
    private Dictionary<string, Type> _classMap;
    public UILayer curLayer;
    private List<UILayer> _popArr;
    public SubLayerMgr()
    {
        _classMap = new Dictionary<string, Type>();
        _popArr = new List<UILayer>();
    }

    /**
    * ע����ҳ��
    * @param layerClass 
    */
    public void register(string layerName, object opt)
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

        this.checkDestoryLastLayer(needDestory);

        if (this.curLayer)
        {
            if (toPush) _popArr.Add(curLayer);
            if (toPush || !needDestory)
            {
                this.curLayer.removeSelf();
            }
        }

        if (registerLayer != null && registerLayer is UILayer)
        {
            this.curLayer = registerLayer;
            this.curLayer.addSelf();
            return;
        }

        this.curLayer = script.show(data);
        if (_classMap[layerName] != null)
        {
            this._classMap[layerName] = curLayer;
        }
    }

    /**�ж������ϸ����沢�ͷ���Դ */
    private void checkDestoryLastLayer(bool destory = false)
    {
        if (destory && this.curLayer && !this.curLayer.hasDestory)
        {
            this.curLayer.close();
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
        _popArr.RemoveAt(_popArr.Count);
        curLayer.addSelf();
    }

    /**�������ע���layer */
    public void releaseAllLayer()
    {
        checkDestoryLastLayer(true);
        for (let i = 0; i < self._popArr.length; i++)
        {
            let layer = this._popArr[i];
            if (!layer.hasDestory) layer.close();
        }

        for (let key in this._classMap)
        {
            let layer = this._classMap[key];
            if (layer.node && !layer.hasDestory)
            {
                layer.close();
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
