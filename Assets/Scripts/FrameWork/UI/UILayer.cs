using FairyGUI;
using System;

public class UILayer : UIComp
{
    /// <summary>
    /// ��ʾUI
    /// </summary>
    /// <param name="pkgName"> ���� </param>
    /// <param name="compName"> �����(��ʵ���ǽű�����)</param>
    /// <param name="data">����</param>
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

    /**��ҳ��ʱ�Ķ��� */
    protected virtual void OnOpenAnimation() { }
    /**�ر�ҳ��ʱ�Ķ��� */
    protected virtual void OnCloseAnimation(GTweenCallback callback)
    {
        if (callback != null) callback();
    }

    /// <summary>
    /// ����ҳ��
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
