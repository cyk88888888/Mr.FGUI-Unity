/// <summary>
/// 游戏事件类型
/// CYK
/// </summary>
public class GameEvent
{
    private static int _next = 0;

    private static string next
    {
        get
        {
            _next++;
            return _next.ToString();
        }
    }
    public static string ChangeGridType = next;//变更格子类型
    public static string ClearGridType = next;//删除指定类型格子
    public static string ClearLineAndGrid = next;//删除所有线条和格子
    public static string ImportMapJson = next;//导入地图json数据
    public static string ResizeGrid = next;//变更格子类型
}
