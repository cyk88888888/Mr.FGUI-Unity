/// <summary>
/// 资源管理器
/// </summary>
public class ResMgr
{
    private static ResMgr _inst;
    public static ResMgr inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = new ResMgr();
            }
            return _inst;
        }
    }
}
