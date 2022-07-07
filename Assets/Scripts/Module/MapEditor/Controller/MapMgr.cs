using FairyGUI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 地图管理器
/// author：cyk
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

    public int mapWidth = 6000;//地图宽
    public int mapHeight = 6000;//地图高
    public int cellSize = 40;//格子大小（默认40）
    public const string ExtensionJson = ".json";//保存文件的后缀名
    public Dictionary<GridType, Dictionary<string, GComponent>> gridTypeDic;//当前地图数据

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

    /** 选择文件夹**/
    public void OpenDirectoryBrower()
    {
        FileUT.OpenDirectoryBrower((string path) => {
            Debug.Log(path);
        });
    }
    /** 选择文件导入**/
    public void ImportJsonData()
    {
        FileUT.OpenFileBrower((string path)=> {
            string json = File.ReadAllText(path);
            Debug.Log(json);
        });
    }
    /** 保存当前地图数据文件**/
    public void ExportJsonData()
    {
        FileUT.SaveFileBrower((string path) => {
            string fullPath = path + ExtensionJson;
        
            Debug.Log(fullPath);
            MapJsonInfo mapInfo = new();
            mapInfo.mapWidth = mapWidth;
            mapInfo.mapHeight = mapHeight;
            mapInfo.gridSize = cellSize;
            float numCols = Mathf.Floor(mapWidth / cellSize);//列
            float numRows = Mathf.Floor(mapHeight / cellSize);//行
            for (int i = 0; i < numRows; i++)
            {
                List<int> linewalkList = new List<int>();//每一行
                mapInfo.walkList.Add(linewalkList);
                for (int j = 0; j < numCols; j++)
                {
                    gridTypeDic.TryGetValue(GridType.Walk, out Dictionary<string, GComponent> walkGridDic);
                    if (walkGridDic == null)
                    {
                        linewalkList.Add(0);
                    }
                    else
                    {
                        walkGridDic.TryGetValue(j + "_" + i, out GComponent gridItem);
                        linewalkList.Add(gridItem == null ? 0 : 1);
                    }
                }
            }
     
            File.WriteAllText(fullPath, JsonConvert.SerializeObject(mapInfo));
            MsgMgr.ShowMsg("导出成功");
        });
    }
}

/** 格子类型**/
public enum GridType
{
    None,
    Walk,
    Block,
    BlockVerts
}
