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

    public int mapWidth = 4000;//地图宽
    public int mapHeight = 4000;//地图高
    public int mapId = 1;//当前地图id
    public int cellSize = 40;//格子大小（默认40）
    public const string ExtensionJson = ".json";//保存文件的后缀名
    public Dictionary<GridType, Dictionary<string, GGraph>> gridTypeDic;//当前地图数据
    public JoystickComp joystick;//当前摇杆
    public List<MapSelectInfo> mapDataList = new()//所有地图信息列表
    {
        new MapSelectInfo("地图1", 1),
        new MapSelectInfo("地图2", 2),
        new MapSelectInfo("地图3", 3),
        new MapSelectInfo("地图4", 4),
        new MapSelectInfo("地图5", 5),
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

    public Color getColorByType(GridType type)
    {
        Color color = Color.white;
        switch (type)
        {
            case GridType.Walk:
                color = Color.green;
                break;
            case GridType.Block:
                color = Color.black;
                break;
            case GridType.BlockVerts:
                color = Color.gray;
                break;
            case GridType.Water:
                color = Color.blue;
                break;
        }
        return color;
    }

    /** 选择文件夹**/
    public void OpenDirectoryBrower()
    {
        FileUT.OpenDirectoryBrower((string path) =>
        {
            Debug.Log(path);
        });
    }
    /** 选择文件导入**/
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
    /** 保存当前地图数据文件**/
    public void ExportJsonData()
    {
        FileUT.SaveFileBrower((string path) =>
        {
            string fullPath = path + ExtensionJson;//保存的json文件数据完整地址
            MapJsonInfo mapInfo = new();
            mapInfo.mapWidth = mapWidth;
            mapInfo.mapHeight = mapHeight;
            mapInfo.cellSize = cellSize;
            mapInfo.mapId = mapId;
            /** 设置行走区域**/
            float numCols = Mathf.Floor(mapWidth / cellSize);//列
            float numRows = Mathf.Floor(mapHeight / cellSize);//行
            for (int i = 0; i < numRows; i++)
            {
                List<int> linewalkList = new List<int>();//每一行
                mapInfo.walkList.Add(linewalkList);
                for (int j = 0; j < numCols; j++)
                {
                    gridTypeDic.TryGetValue(GridType.Walk, out Dictionary<string, GGraph> walkGridDic);
                    if (walkGridDic == null)
                    {
                        linewalkList.Add(1);
                    }
                    else
                    {
                        walkGridDic.TryGetValue(j + "_" + i, out GGraph gridItem);
                        linewalkList.Add(gridItem != null ? 1 : 0);
                    }
                }
            }

            /** 设置障碍物**/
            AddBlockByType(GridType.Block);
            AddBlockByType(GridType.BlockVerts);
            AddBlockByType(GridType.Water);
            void AddBlockByType(GridType gridType)
            {
                gridTypeDic.TryGetValue(gridType, out Dictionary<string, GGraph> blockGridDic);
                if (blockGridDic != null)
                {
                    foreach (var item in blockGridDic)
                    {
                        List<int> newList = new List<int>();
                        if (gridType == GridType.Block) mapInfo.blockList.Add(newList);
                        else if(gridType == GridType.BlockVerts) mapInfo.blockVertList.Add(newList);
                        else if (gridType == GridType.Water) mapInfo.waterList.Add(newList);
                        string[] splitArr = item.Key.Split("_");
                        newList.Add(int.Parse(splitArr[0]));
                        newList.Add(int.Parse(splitArr[1]));
                    }
                }
            }

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(mapInfo));
            MsgMgr.ShowMsg("导出成功");
        });
    }

    public void ShowMapPreviewDlg(GObject aObject)
    {
        //用RenderTexture截图显示的话会比较模糊
        //DisplayObject dObject = grp_container.displayObject;
        //dObject.EnterPaintingMode(1024, null);
        ////纹理将在本帧渲染后才能更新，所以访问纹理的代码需要延迟到下一帧执行。
        //Timers.inst.CallLater((object param) =>
        //{

        //    RenderTexture renderTexture = (RenderTexture)dObject.paintingGraphics.texture.nativeTexture;
        //    UILayer.Show("MapPreviewDlg", renderTexture);
        //});

        JuHuaDlg juahua = (JuHuaDlg)UILayer.Show("JuHuaDlg");
        //先保存图片到本地，再读取为Texture2D格式。这种方式显示会更清晰
        BaseUT.SaveViewShotToLocal(aObject, (string path) =>
        {
            //创建文件读取流
            FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);

            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];

            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            //释放文件读取流
            fileStream.Close();

            fileStream.Dispose();
            fileStream = null;

            //创建Texture

            Texture2D texture = new(mapWidth, mapHeight);
            texture.LoadImage(bytes);
            juahua.Close();
            UILayer.Show("MapPreviewDlg", texture);
        }, false, "MapPreview");
    }
}

/** 格子类型**/
public enum GridType
{
    None,
    Walk,
    Block,
    BlockVerts,
    Water
}
