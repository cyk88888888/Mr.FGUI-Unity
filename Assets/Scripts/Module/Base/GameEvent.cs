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
    public static string ChangeMap = next;//切换地图
    public static string ImportMapJson = next;//导入地图json数据
    public static string ResizeGrid = next;//变更格子大小
    public static string ResizeMap = next;//变更地图大小
    public static string ScreenShoot = next;//截图绘画区域
    public static string RunDemo = next;//运行demo
    public static string CloseDemo = next;//关闭demo
    public static string ToCenter = next;//到地图中心点
    public static string ToOriginalScale = next;//回归原大小缩放
    public static string ClearAllData = next;//清除所有数据
    
}
