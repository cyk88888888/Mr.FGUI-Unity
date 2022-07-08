using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 地图json数据结构
/// author：cyk
/// </summary>
public class MapJsonInfo
{
    public int mapWidth;//地图宽
    public int mapHeight;//地图高
    public int mapId;//地图id
    public int cellSize;//格子大小
    public List<List<int>> walkList;//格子可行走状态列表（1可行走，0不可行走）
    public List<List<int>> blockList;//格子障碍物列表
    public List<List<int>> blockVertList;//格子障碍物顶点列表
    public List<List<int>> waterList;//格子水区域列表
    public MapJsonInfo()
    {
        walkList = new List<List<int>>();
        blockList = new List<List<int>>();
        blockVertList = new List<List<int>>();
        waterList = new List<List<int>>();
    }
}

