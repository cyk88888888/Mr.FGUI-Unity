using FairyGUI;

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
        layer.GetParent().AddChild(layer);
        BaseUT.Inst.SetFitSize(layer.view);
        if (data != null) layer.SetData(data);
        return layer;
    }

    protected virtual GComponent GetParent()
    {
        return SceneMgr.inst.curScene.layer;
    }
}
