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
    public Dictionary<GridType, Dictionary<string, GGraph>> gridTypeDic;//��ǰ��ͼ����
    public JoystickComp joystick;//��ǰҡ��
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

            /** �����ϰ���**/
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
            MsgMgr.ShowMsg("�����ɹ�");
        });
    }

    public void ShowMapPreviewDlg(GObject aObject)
    {
        //��RenderTexture��ͼ��ʾ�Ļ���Ƚ�ģ��
        //DisplayObject dObject = grp_container.displayObject;
        //dObject.EnterPaintingMode(1024, null);
        ////�����ڱ�֡��Ⱦ����ܸ��£����Է�������Ĵ�����Ҫ�ӳٵ���һִ֡�С�
        //Timers.inst.CallLater((object param) =>
        //{

        //    RenderTexture renderTexture = (RenderTexture)dObject.paintingGraphics.texture.nativeTexture;
        //    UILayer.Show("MapPreviewDlg", renderTexture);
        //});

        JuHuaDlg juahua = (JuHuaDlg)UILayer.Show("JuHuaDlg");
        //�ȱ���ͼƬ�����أ��ٶ�ȡΪTexture2D��ʽ�����ַ�ʽ��ʾ�������
        BaseUT.SaveViewShotToLocal(aObject, (string path) =>
        {
            //�����ļ���ȡ��
            FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);

            //�����ļ����Ȼ�����
            byte[] bytes = new byte[fileStream.Length];

            //��ȡ�ļ�
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            //�ͷ��ļ���ȡ��
            fileStream.Close();

            fileStream.Dispose();
            fileStream = null;

            //����Texture

            Texture2D texture = new(mapWidth, mapHeight);
            texture.LoadImage(bytes);
            juahua.Close();
            UILayer.Show("MapPreviewDlg", texture);
        }, false, "MapPreview");
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
