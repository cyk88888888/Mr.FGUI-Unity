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
        layer.gameObjectName = layerName + "_script";
        layer.AddToLayer();
        if (data != null) layer.SetData(data);
        return layer;
    }

    public virtual GComponent GetParent()
    {
        return SceneMgr.inst.curScene.layer;
    }
    /// <summary>
    /// ��ӵ��㼶
    /// </summary>
    protected void AddToLayer()
    {
        SetParent(GetParent());
    }
}
