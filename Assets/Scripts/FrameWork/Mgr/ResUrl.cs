/// <summary>
/// 资源路径静态类
/// </summary>
public class ResUrl
{
    /// <summary>
    /// 获取道具图标资源路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetItemIcon(int id)
    {
        return "Dy/Icon/i" + id;
    }
}
