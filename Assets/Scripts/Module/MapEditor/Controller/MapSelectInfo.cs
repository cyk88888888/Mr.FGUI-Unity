using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 地图选择数据结构
/// author：cyk
/// </summary>
public class MapSelectInfo
{
    public string mapName;//地图名称
    public int mapId;//mapid
    public MapSelectInfo(string _mapName, int _mapId)
    {
        mapName = _mapName;
        mapId = _mapId;
    }
}

