using FairyGUI;

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
    /// 添加到层级
    /// </summary>
    protected void AddToLayer()
    {
        SetParent(GetParent());
    }
}
