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
    * 注册子页面
    * @param layerClass 
    */
    public void register(string layerName, object opt)
    {
        _classMap[layerName] = Type.GetType(layerName);
    }

    /**显示指定界面（替换模式） */
    public void run(string layerName, object data = null)
    {
        _show(layerName, data);
    }

    /**显示指定界面（入栈模式） */
    public void push(string layerName, data?: any)
    {
        _show(layerName, data, true);
    }

    private void _show(string layerName, data?: any, toPush?: boolean)
    {
        let script: any = typeof LayerNameOrClass === 'string' ? js.getClassByName(LayerNameOrClass) : LayerNameOrClass;

        if (this.curLayer && this.curLayer.className == layerName) return;//打开同个界面

        let registerLayer = this._classMap[layerName];
        let needDestory = !registerLayer && !toPush;//未注册  && 非入栈模式

        this.checkDestoryLastLayer(needDestory);

        if (this.curLayer)
        {
            if (toPush) this._popArr.push(this.curLayer);
            if (toPush || !needDestory)
            {
                this.curLayer.removeSelf();
            }
        }

        if (registerLayer && registerLayer.node)
        {
            this.curLayer = registerLayer;
            this.curLayer.addSelf();
            return;
        }

        this.curLayer = script.show(data);
        if (this._classMap[layerName])
        {
            this._classMap[layerName] = this.curLayer;
        }
    }

    /**判断销毁上个界面并释放资源 */
    private void checkDestoryLastLayer(bool destory = false)
    {
        if (destory && this.curLayer && !this.curLayer.hasDestory)
        {
            this.curLayer.close();
        }
    }

    /** layer出栈*/
    public void pop()
    {
        if (self._popArr.length <= 0)
        {
            console.error('已经pop到底了！！！！！！！');
            return;
        }
        self.checkDestoryLastLayer(true);
        self.curLayer = self._popArr.pop();
        self.curLayer.addSelf();
    }

    /**清除所有注册的layer */
    public void releaseAllLayer()
    {
        let self = this;
        this.checkDestoryLastLayer(true);
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

        self._popArr = [];
    }

    public void dispose()
    {
        let self = this;
        self.releaseAllLayer();
        self._classMap = null;
        self._popArr = null;
    }
}
