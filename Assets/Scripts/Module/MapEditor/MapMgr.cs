using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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

    public void OpenDirectoryBrower()
    {
        FileUT.OpenDirectoryBrower((string path) => {
            Debug.Log(path);
        });
    }
    
    public void ImportJsonData()
    {
        FileUT.OpenFileBrower((string path)=> {
            string json = File.ReadAllText(path);
            Debug.Log(json);
        });
    }

    public void ExportJsonData()
    {
        FileUT.SaveFileBrower((string path) => {
            string fullPath = path + ExtensionJson;
            File.WriteAllText(fullPath, JsonConvert.SerializeObject(new List<int>() { 111,222,333}));
            Debug.Log(fullPath);
            MsgMgr.ShowMsg("�����ɹ�");
        });
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
