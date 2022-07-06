using System.Collections.Generic;
/// <summary>
/// µØÍ¼±à¼­Æ÷³¡¾°
/// </summary>
public class MapEditorScene : UIScene
{
    protected override void Ctor()
    {
        mainClassLayer = "MapEditorLayer";
        List<string> classList = new();
        foreach (var item in classList)
        {
            subLayerMgr.Register(item);
        }
    }
}
