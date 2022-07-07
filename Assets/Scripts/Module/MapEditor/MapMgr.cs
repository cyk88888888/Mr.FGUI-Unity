/// <summary>
/// ��ͼ������
/// author��cyk
/// </summary>
public class MapMgr
{
    private static MapMgr _inst;
    public static MapMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new MapMgr();
            }
            return _inst;
        }
    }

    /// <summary>
    /// ���Ӵ�С��Ĭ��40��
    /// </summary>
    public static int cellSize = 40;
    public const string ExtensionJson = ".json";//json��׺��
    public string getGridUrlByType(GridType type)
    {
        string url = "";
        switch (type)
        {
            case GridType.Walk:
                url = "ui://MapEditor/green";
                break;
            case GridType.Block:
                url = "ui://MapEditor/black";
                break;
            case GridType.BlockVerts:
                url = "ui://MapEditor/red";
                break;
        }
        return url;
    }
}

/** ��������**/
public enum GridType
{
    None,
    Walk,
    Block,
    BlockVerts
}
