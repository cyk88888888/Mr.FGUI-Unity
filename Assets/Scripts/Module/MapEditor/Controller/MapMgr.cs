using FairyGUI;
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

    public int mapWidth = 4000;//��ͼ��
    public int mapHeight = 4000;//��ͼ��
    public int mapId = 1;//��ǰ��ͼid
    public int cellSize = 40;//���Ӵ�С��Ĭ��40��
    public const string ExtensionJson = ".json";//�����ļ��ĺ�׺��
    public Dictionary<GridType, Dictionary<string, GComponent>> gridTypeDic;//��ǰ��ͼ����
    public List<MapSelectInfo> mapDataList = new()//���е�ͼ��Ϣ�б�
    {
        new MapSelectInfo("��ͼ1", 1),
        new MapSelectInfo("��ͼ2", 2),
        new MapSelectInfo("��ͼ3", 3),
        new MapSelectInfo("��ͼ4", 4),
        new MapSelectInfo("��ͼ5", 5),
    };

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
                url = "ui://MapEditor/redCircle";
                break;
            case GridType.Water:
                url = "ui://MapEditor/blue";
                break;
        }
        return url;
    }

    /** ѡ���ļ���**/
    public void OpenDirectoryBrower()
    {
        FileUT.OpenDirectoryBrower((string path) =>
        {
            Debug.Log(path);
        });
    }
    /** ѡ���ļ�����**/
    public void ImportJsonData()
    {
        FileUT.OpenFileBrower((string path) =>
        {
            string json = File.ReadAllText(path);
            MapJsonInfo mapInfo = JsonConvert.DeserializeObject<MapJsonInfo>(json);
            mapWidth = mapInfo.mapWidth;
            mapHeight = mapInfo.mapHeight;
            cellSize = mapInfo.cellSize;
            mapId = mapInfo.mapId;
            Global.GlobalEmmiter.Emit(GameEvent.ImportMapJson, new object[] { mapInfo });
            Debug.Log(json);
        });
    }
    /** ���浱ǰ��ͼ�����ļ�**/
    public void ExportJsonData()
    {
        FileUT.SaveFileBrower((string path) =>
        {
            string fullPath = path + ExtensionJson;//�����json�ļ�����������ַ
            MapJsonInfo mapInfo = new();
            mapInfo.mapWidth = mapWidth;
            mapInfo.mapHeight = mapHeight;
            mapInfo.cellSize = cellSize;
            mapInfo.mapId = mapId;
            /** ������������**/
            float numCols = Mathf.Floor(mapWidth / cellSize);//��
            float numRows = Mathf.Floor(mapHeight / cellSize);//��
            for (int i = 0; i < numRows; i++)
            {
                List<int> linewalkList = new List<int>();//ÿһ��
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
     
            /** �����ϰ���**/
            AddBlockByType(GridType.Block);
            AddBlockByType(GridType.BlockVerts);
            AddBlockByType(GridType.Water);
            void AddBlockByType(GridType gridType) {
                gridTypeDic.TryGetValue(gridType, out Dictionary<string, GComponent> blockGridDic);
                if (blockGridDic != null)
                {
                    foreach (var item in blockGridDic)
                    {
                        List<int> newList = new List<int>();
                        if(gridType == GridType.Block) mapInfo.blockList.Add(newList);
                        else mapInfo.blockVertList.Add(newList);

                        string[] splitArr = item.Key.Split("_");
                        newList.Add(int.Parse(splitArr[0]));
                        newList.Add(int.Parse(splitArr[1]));
                    }
                }
            }

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(mapInfo));
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
    BlockVerts,
    Water
}
