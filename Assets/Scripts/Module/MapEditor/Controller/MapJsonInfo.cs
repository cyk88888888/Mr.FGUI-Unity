using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// ��ͼjson���ݽṹ
/// author��cyk
/// </summary>
public class MapJsonInfo
{
    public int mapWidth;//��ͼ��
    public int mapHeight;//��ͼ��
    public int mapId;//��ͼid
    public int cellSize;//���Ӵ�С
    public List<List<int>> walkList;//���ӿ�����״̬�б�1�����ߣ�0�������ߣ�
    public List<List<int>> blockList;//�����ϰ����б�
    public List<List<int>> blockVertList;//�����ϰ��ﶥ���б�
    public List<List<int>> waterList;//����ˮ�����б�
    public MapJsonInfo()
    {
        walkList = new List<List<int>>();
        blockList = new List<List<int>>();
        blockVertList = new List<List<int>>();
        waterList = new List<List<int>>();
    }
}

