using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// ��ͼѡ�����ݽṹ
/// author��cyk
/// </summary>
public class MapSelectInfo
{
    public string mapName;//��ͼ����
    public int mapId;//mapid
    public MapSelectInfo(string _mapName, int _mapId)
    {
        mapName = _mapName;
        mapId = _mapId;
    }
}

