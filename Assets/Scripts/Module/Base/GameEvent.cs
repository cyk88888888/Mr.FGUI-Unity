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
    public static string ResizeGrid = next;//变更格子类型
}
