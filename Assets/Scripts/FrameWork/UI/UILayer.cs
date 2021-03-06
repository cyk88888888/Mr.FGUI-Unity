using FairyGUI;
using System;

public class UILayer : UIComp
{
    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="pkgName"> 包名 </param>
    /// <param name="compName"> 组件名(其实就是脚本名称)</param>
    /// <param name="data">数据</param>
    /// <returns></returns>
    public static UILayer Show(string layerName, object data = null)
    {
        UILayer layer = BaseUT.Inst.CreateClassByName<UILayer>(layerName);
        layer.sameSizeWithView = false;
        layer.gameObjectName = layerName + "_script";
        BaseUT.Inst.SetFitSize(layer);
        if (data != null) layer.SetData(data);
        layer.GetParent().AddChild(layer);
        BaseUT.Inst.SetFitSize(layer.view);
        layer.OnAddToLayer();
        return layer;
    }

    protected virtual GComponent GetParent()
    {
        return SceneMgr.inst.curScene.layer;
    }

    protected virtual void OnAddToLayer() { }

    /**打开页面时的动画 */
    protected virtual void OnOpenAnimation() { }
    /**关闭页面时的动画 */
    protected virtual void OnCloseAnimation(GTweenCallback callback)
    {
        if (callback != null) callback();
    }

    /// <summary>
    /// 销毁页面
    /// </summary>
    public void Close()
    {
        OnCloseAnimation(() =>
        {
            _closeCb?.Invoke();
            __dispose();
            Destory();
        });
    }

    private Action _closeCb;
    public void OnClose(Action cb)
    {
        _closeCb = cb;
    }

}
